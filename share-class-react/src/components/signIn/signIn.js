import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

const SignIn = ({ user, setUser }) => {
  const [errorMessages, setErrorMessages] = useState([]);
  const navigate = useNavigate();

  const signIn = async (event) => {
    event.preventDefault();

    var { login, password } = document.forms[0];

    const requestOptions = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        login: login.value,
        password: password.value,
      }),
    };
    return await fetch("api/Account/SignIn", requestOptions)
      .then((response) => {
        if (response.status == 200)
          setUser({ isAuthenticated: true, userName: "" });

        return response.json();
      })
      .then(
        (data) => {
          console.log("Data: ", data);
          if (
            typeof data !== "undefined" &&
            typeof data.userName !== "undefined"
          ) {
            setUser({ isAuthenticated: true, userName: data.userName });
            navigate("/");
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
          <h3>SignIn</h3>
          <form onSubmit={signIn}>
            <label>Login </label>
            <input type="text" name="login" placeholder="Login" />
            <br />
            <label>Password </label>
            <input type="text" name="password" placeholder="Password" />
            <br />
            <button type="submit">SignIn</button>
          </form>
          {renderErrorMessage()}
        </>
      )}
    </>
  );
};

export default SignIn;
