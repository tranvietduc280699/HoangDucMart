
<template>
  <div class="loginpage">
    <div class="loginbox">

      <div class="loginlogo"></div>

      <div class="logintitle">
        Đăng nhập
      </div>

      <div class="logindesc">
        Chào mừng bạn quay trở lại!
      </div>

      <div class="loginform">
        <div class="inputgroup">
          <div class="inputlabel">Tên đăng nhập</div>
          <input
            type="text"
            v-model="userLogin.username"
            placeholder="Nhập tên đăng nhập"
          />
        </div>

        <div class="inputgroup">
          <div class="inputlabel">Mật khẩu</div>
          <input
            type="password"
            v-model="userLogin.password"
            placeholder="Nhập mật khẩu"
          />
        </div>
          <div class="forgotpass">
            Quên mật khẩu?
          </div>

        <input
          type="button"
          class="loginbutton"
          value="ĐĂNG NHẬP"
          @click="handleLogin"
        />
      </div>
      <div class="register-prompt">
        Bạn chưa có tài khoản? <span class="register-link">Đăng ký ngay</span>
      </div>
      <div class="loginfooter">
        © 2026 - Hoang Duc Mart. All rights reserved.
      </div>

    </div>
  </div>
</template>
<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { UserLogin } from '@/models';
import { userApi } from '@/api/userApi/userApi.js';
import { useToast } from '@/composables/useToast';

const { showToast } = useToast();
const router = useRouter();

const userLogin = ref(new UserLogin());  
const errorMessage = ref('');        

async function handleLogin() {
  errorMessage.value = '';
  try {
    const result = await userApi.login(userLogin.value);
    if(result.code !== 200) {
      showToast('Đăng nhập thất bại. Vui lòng kiểm tra lại thông tin.', 'error');
      return;
    }
    // lưu trữ thông tin đăng nhập vào localStorage
    localStorage.setItem('accessToken', result.data.accessToken);
    localStorage.setItem('user', JSON.stringify(result.data.user));
    router.push('/');
  } catch (error: any) {
    errorMessage.value = error.message;   
  }
}
</script>


<style scoped lang="scss">

.loginpage {
  width: 100%;
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  box-sizing: border-box;

  .loginbox {
    width: 100%;
    max-width: 420px;
    background: var(--surface);
    padding: 40px;
    border: 1px solid var(--line);
    border-radius: 16px;
    box-sizing: border-box;
    box-shadow: 0 10px 40px rgba(28, 42, 34, 0.08);
  }

  .loginlogo {
    width: 75px;
    height: 75px;
    margin: 0 auto 20px;
    background-image: var(--icon-logo);
    background-repeat: no-repeat;
    background-position: center;
    background-size: contain;
  }

  .logintitle {
    font-size: 27px;
    font-weight: 700;
    color: var(--ink);
    text-align: center;
    margin-bottom: 8px;
  }

  .logindesc {
    font-size: 14px;
    color: var(--muted);
    text-align: center;
    margin-bottom: 32px;
  }

  .loginform {
    width: 100%;
  }

  .inputgroup {
    margin-bottom: 22px;
  }

  .inputlabel {
    font-size: 14px;
    font-weight: 600;
    color: var(--text);
    margin-bottom: 9px;
  }

  input[type="text"],
  input[type="password"] {
    width: 100%;
    height: 48px;
    padding: 0 14px;
    border: 1px solid var(--border);
    border-radius: 8px;
    outline: none;
    box-sizing: border-box;
    background: var(--surface);
    color: var(--text);
    font-size: 14px;
    transition: 0.2s;

    &:focus {
      border-color: var(--brand);
      box-shadow: 0 0 0 3px rgba(23, 88, 60, 0.1);
    }

    &::placeholder {
      color: var(--muted);
    }
  }

  .forgotpass, .registerAccount {
    text-align: right;
    font-size: 13px;
    color: var(--blue);
    cursor: pointer;
    margin-bottom: 12px;

    &:hover {
      color: var(--blue-dark);
    }
  }

  .loginbutton {
    width: 100%;
    height: 48px;
    border: none;
    border-radius: 8px;
    background: var(--brand);
    color: var(--brand-ink);
    font-size: 15px;
    font-weight: 700;
    cursor: pointer;
    transition: 0.2s;

    &:hover {
      background: var(--blue-dark);
    }
  }
  .register-prompt {
    text-align: center;
    font-size: 13px;
    color: var(--muted);
    margin-top: 20px;

    .register-link {
      color: var(--brand);
      font-weight: 600;
      cursor: pointer;

      &:hover {
        text-decoration: underline;
      }
    }
  }
  .loginfooter {
    text-align: center;
    margin-top: 30px;
    font-size: 12px;
    color: var(--muted);
  }
}

@media (max-width: 480px) {
  .loginpage {
    .loginbox {
      padding: 28px 22px;
    }
  }
}

</style>