
CREATE PROCEDURE Proc_Order_Insert(
    IN p_OrderId         VARCHAR(36),
    IN p_UserId          VARCHAR(36),
    IN p_OrderCode       VARCHAR(50),
    IN p_OrderStatus     INT,
    IN p_PaymentStatus   INT,
    IN p_PaymentMethod   VARCHAR(50),
    IN p_TotalAmount     DECIMAL(15,2),
    IN p_DiscountAmount  DECIMAL(15,2),
    IN p_FinalAmount     DECIMAL(15,2),
    IN p_ReceiverName    VARCHAR(150),
    IN p_ReceiverPhone   VARCHAR(20),
    IN p_ShippingAddress TEXT,
    IN p_Note            TEXT,
    IN p_VatRate         DECIMAL(5,2),
    IN p_VatAmount       DECIMAL(15,2),
    IN p_CreatedAt       DATETIME(6),
    IN p_CreatedBy       VARCHAR(36),
    IN p_DetailsJson     JSON
)
BEGIN
    -- Có lỗi thì rollback rồi ném lỗi ra cho C# bắt
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    -- 1. Insert đơn hàng
    INSERT INTO URA_ORDERS (
        ORDER_ID, USER_ID, ORDER_CODE, ORDER_STATUS, PAYMENT_STATUS, PAYMENT_METHOD,
        TOTAL_AMOUNT, DISCOUNT_AMOUNT, FINAL_AMOUNT,
        RECEIVER_NAME, RECEIVER_PHONE, SHIPPING_ADDRESS, NOTE,
        VAT_RATE, VAT_AMOUNT,
        CREATED_AT, CREATED_BY
    )
    VALUES (
        p_OrderId, p_UserId, p_OrderCode, p_OrderStatus, p_PaymentStatus, p_PaymentMethod,
        p_TotalAmount, p_DiscountAmount, p_FinalAmount,
        p_ReceiverName, p_ReceiverPhone, p_ShippingAddress, p_Note,
        p_VatRate, p_VatAmount,
        p_CreatedAt, p_CreatedBy
    );

    -- 2. Insert các dòng chi tiết từ JSON
    INSERT INTO URA_ORDER_DETAILS (
        ORDER_DETAIL_ID, ORDER_ID, PRODUCT_ID,
        PRODUCT_NAME, SKU, UNIT,
        QUANTITY, UNIT_PRICE, TOTAL_PRICE,
        CREATED_AT, CREATED_BY,
        VAT_RATE, VAT_AMOUNT
    )
    SELECT
        jt.OrderDetailId, p_OrderId, jt.ProductId,
        jt.ProductName, jt.Sku, jt.Unit,
        jt.Quantity, jt.UnitPrice, jt.TotalPrice,
        p_CreatedAt, p_CreatedBy,
        jt.VatRate, jt.VatAmount
    FROM JSON_TABLE(
        p_DetailsJson,
        '$[*]' COLUMNS (
            OrderDetailId VARCHAR(36)   PATH '$.OrderDetailId',
            ProductId     VARCHAR(36)   PATH '$.ProductId',
            ProductName   VARCHAR(200)  PATH '$.ProductName',
            Sku           VARCHAR(64)   PATH '$.Sku',
            Unit          VARCHAR(30)   PATH '$.Unit',
            Quantity      DECIMAL(15,3) PATH '$.Quantity',
            UnitPrice     DECIMAL(15,2) PATH '$.UnitPrice',
            TotalPrice    DECIMAL(15,2) PATH '$.TotalPrice',
            VatRate       DECIMAL(5,2)  PATH '$.VatRate',
            VatAmount     DECIMAL(15,2) PATH '$.VatAmount'
        )
    ) AS jt;

    COMMIT;
END

-- cập nhật đơn hàng
CREATE PROCEDURE Proc_Order_Update(
    IN p_OrderId         VARCHAR(36),
    IN p_UserId          VARCHAR(36),
    IN p_PaymentMethod   VARCHAR(50),
    IN p_TotalAmount     DECIMAL(15,2),
    IN p_DiscountAmount  DECIMAL(15,2),
    IN p_FinalAmount     DECIMAL(15,2),
    IN p_ReceiverName    VARCHAR(150),
    IN p_ReceiverPhone   VARCHAR(20),
    IN p_ShippingAddress TEXT,
    IN p_Note            TEXT,
    IN p_VatRate         DECIMAL(5,2),
    IN p_VatAmount       DECIMAL(15,2),
    IN p_UpdatedAt       DATETIME(6),
    IN p_UpdatedBy       VARCHAR(36),
    IN p_DetailsJson     JSON
)
BEGIN
    DECLARE v_status INT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    -- Khóa dòng và kiểm tra lại trạng thái
    SELECT PAYMENT_STATUS INTO v_status
    FROM URA_ORDERS
    WHERE ORDER_ID = p_OrderId
    FOR UPDATE;

    IF v_status IS NULL OR v_status <> 2 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Đơn hàng không ở trạng thái cho phép sửa';
    END IF;

    UPDATE URA_ORDERS
    SET USER_ID          = p_UserId,
        PAYMENT_METHOD   = p_PaymentMethod,
        TOTAL_AMOUNT     = p_TotalAmount,
        DISCOUNT_AMOUNT  = p_DiscountAmount,
        FINAL_AMOUNT     = p_FinalAmount,
        RECEIVER_NAME    = p_ReceiverName,
        RECEIVER_PHONE   = p_ReceiverPhone,
        SHIPPING_ADDRESS = p_ShippingAddress,
        NOTE             = p_Note,
        VAT_RATE         = p_VatRate,
        VAT_AMOUNT       = p_VatAmount,
        UPDATED_AT       = p_UpdatedAt,
        UPDATED_BY       = p_UpdatedBy
    WHERE ORDER_ID = p_OrderId;

    -- Xóa detail cũ, insert lại danh sách mới
    DELETE FROM URA_ORDER_DETAILS WHERE ORDER_ID = p_OrderId;

    INSERT INTO URA_ORDER_DETAILS (
        ORDER_DETAIL_ID, ORDER_ID, PRODUCT_ID, PRODUCT_NAME, SKU, UNIT,
        QUANTITY, UNIT_PRICE, TOTAL_PRICE,
        CREATED_AT, CREATED_BY, VAT_RATE, VAT_AMOUNT
    )
    SELECT jt.OrderDetailId, p_OrderId, jt.ProductId, jt.ProductName, jt.Sku, jt.Unit,
           jt.Quantity, jt.UnitPrice, jt.TotalPrice,
           p_UpdatedAt, p_UpdatedBy, jt.VatRate, jt.VatAmount
    FROM JSON_TABLE(
        p_DetailsJson,
        '$[*]' COLUMNS (
            OrderDetailId VARCHAR(36)   PATH '$.OrderDetailId',
            ProductId     VARCHAR(36)   PATH '$.ProductId',
            ProductName   VARCHAR(200)  PATH '$.ProductName',
            Sku           VARCHAR(64)   PATH '$.Sku',
            Unit          VARCHAR(30)   PATH '$.Unit',
            Quantity      DECIMAL(15,3) PATH '$.Quantity',
            UnitPrice     DECIMAL(15,2) PATH '$.UnitPrice',
            TotalPrice    DECIMAL(15,2) PATH '$.TotalPrice',
            VatRate       DECIMAL(5,2)  PATH '$.VatRate',
            VatAmount     DECIMAL(15,2) PATH '$.VatAmount'
        )
    ) AS jt;
    COMMIT;
END


-- tạo proc cho công nợ

CREATE PROCEDURE Proc_Debt_Insert(
    IN p_DebtId           VARCHAR(36),
    IN p_OrderId          VARCHAR(36),
    IN p_UserId           VARCHAR(36),
    IN p_DebtorName       VARCHAR(150),
    IN p_DebtorPhone      VARCHAR(20),
    IN p_DebtorAddress    TEXT,
    IN p_DebtType         INT,
    IN p_TotalAmount      DECIMAL(15,2),
    IN p_PaidAmount       DECIMAL(15,2),
    IN p_RemainingAmount  DECIMAL(15,2),
    IN p_DebtStatus       INT,
    IN p_Note             TEXT,
    IN p_CreatedAt        DATETIME(6),
    IN p_CreatedBy        VARCHAR(36),
    IN p_DetailsJson      JSON
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;
    
	IF p_OrderId IS NOT NULL THEN
	    UPDATE URA_ORDERS
	    SET PAYMENT_STATUS = 4, UPDATED_AT = p_CreatedAt, UPDATED_BY = p_CreatedBy
	    WHERE ORDER_ID = p_OrderId;
	END IF;
    -- Insert công nợ
    INSERT INTO URA_DEBTS (
        DEBT_ID, ORDER_ID, USER_ID,
        DEBTOR_NAME, DEBTOR_PHONE, DEBTOR_ADDRESS,
        DEBT_TYPE, TOTAL_AMOUNT, PAID_AMOUNT, REMAINING_AMOUNT,
        DEBT_STATUS, NOTE,
        CREATED_AT, CREATED_BY
    )
    VALUES (
        p_DebtId, p_OrderId, p_UserId,
        p_DebtorName, p_DebtorPhone, p_DebtorAddress,
        p_DebtType, p_TotalAmount, p_PaidAmount, p_RemainingAmount,
        p_DebtStatus, p_Note,
        p_CreatedAt, p_CreatedBy
    );

    -- Insert các dòng chi tiết từ JSON
    INSERT INTO URA_DEBT_DETAILS (
        DEBT_DETAIL_ID, DEBT_ID, PRODUCT_ID,
        PRODUCT_NAME, SKU, UNIT,
        QUANTITY, UNIT_PRICE, TOTAL_PRICE,
        NOTE, CREATED_AT, CREATED_BY
    )
    SELECT
        jt.DebtDetailId, p_DebtId, jt.ProductId,
        jt.ProductName, jt.Sku, jt.Unit,
        jt.Quantity, jt.UnitPrice, jt.TotalPrice,
        jt.Note, p_CreatedAt, p_CreatedBy
    FROM JSON_TABLE(
        p_DetailsJson,
        '$[*]' COLUMNS (
            DebtDetailId VARCHAR(36)   PATH '$.DebtDetailId',
            ProductId    VARCHAR(36)   PATH '$.ProductId',
            ProductName  VARCHAR(200)  PATH '$.ProductName',
            Sku          VARCHAR(64)   PATH '$.Sku',
            Unit         VARCHAR(30)   PATH '$.Unit',
            Quantity     DECIMAL(15,3) PATH '$.Quantity',
            UnitPrice    DECIMAL(15,2) PATH '$.UnitPrice',
            TotalPrice   DECIMAL(15,2) PATH '$.TotalPrice',
            Note         TEXT          PATH '$.Note'
        )
    ) AS jt;
	 -- Nợ từ đơn: chuyển đơn sang trạng thái công nợ 
	IF p_OrderId IS NOT NULL THEN
	    UPDATE URA_ORDERS
	    SET PAYMENT_STATUS = 4,
	        UPDATED_AT     = p_CreatedAt,
	        UPDATED_BY     = p_CreatedBy
	    WHERE ORDER_ID = p_OrderId
	      AND PAYMENT_STATUS IN (1, 2); 

	    -- Không cập nhật được dòng nào
	    IF ROW_COUNT() = 0 THEN
	        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Đơn hàng không thể chuyển sang công nợ';
	    END IF;
	END IF;
    COMMIT;
END


