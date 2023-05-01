import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button, Form, Input, notification } from "antd";

const SignUp = ({ user, setUser }) => {
  const [errorMessages, setErrorMessages] = useState([]);
  const navigate = useNavigate();

  const signUp = async (formValues) => {
    const requestOptions = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        login: formValues.login,
        name: formValues.name,
        password: formValues.password,
        passwordConfirm: formValues.passwordConfirm,
      }),
    };
    return await fetch("/api/Account/SignUp", requestOptions)
      .then((response) => {
        if (response.status == 200)
          setUser({ isAuthenticated: true, userName: "", userRole: "" });

        return response.json();
      })
      .then(
        (data) => {
          console.log("Data: ", data);
          if (
            typeof data !== "undefined" &&
            typeof data.userName !== "undefined"
          ) {
            setUser({
              isAuthenticated: true,
              userName: data.userName,
              userRole: data.userRole,
              userId: data.userId,
            });
            navigate("/");
          } else {
            notification.error({
              message: "Error",
              description: data.message,
            });
          }
          typeof data !== "undefined" &&
            typeof data.error !== "undefined" &&
            setErrorMessages(data.error);
        },
        (error) => {
          console.log(error);
        }
      );
  };

  const renderErrorMessage = () =>
    errorMessages.map((error, index) => <div key={index}>{error}</div>);

  return (
    <>
      {user.isAuthenticated ? (
        <h3>Пользователь {user.userName} успешно вошел в систему</h3>
      ) : (
        <>
          <h3>SignUp</h3>

          <Form
            onFinish={signUp}
            labelCol={{ span: 8 }}
            wrapperCol={{ span: 16 }}
            style={{ maxWidth: 600 }}
            initialValues={{ remember: true }}
            onFinishFailed={renderErrorMessage}
            autoComplete="off"
          >
            <Form.Item
              label="Login"
              name="login"
              rules={[
                { required: true, message: "Please input your username!" },
              ]}
            >
              <Input />
            </Form.Item>

            <Form.Item
              label="Name"
              name="name"
              rules={[{ required: true, message: "Please input your name!" }]}
            >
              <Input />
            </Form.Item>

            <Form.Item
              label="Password"
              name="password"
              rules={[
                { required: true, message: "Please input your password!" },
              ]}
            >
              <Input.Password />
            </Form.Item>

            <Form.Item
              label="Confirm Password"
              name="passwordConfirm"
              rules={[
                { required: true, message: "Please confirm your password!" },
              ]}
            >
              <Input.Password />
            </Form.Item>

            <Form.Item name="errorMessage" wrapperCol={{ offset: 8, span: 16 }}>
              {renderErrorMessage()}
            </Form.Item>

            <Form.Item wrapperCol={{ offset: 8, span: 16 }}>
              <Button type="primary" htmlType="submit">
                Submit
              </Button>
            </Form.Item>
          </Form>
        </>
      )}
    </>
  );
};

export default SignUp;
