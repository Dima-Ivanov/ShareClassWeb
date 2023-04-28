import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

const SignUp = ({ user, setUser }) => {
  const [errorMessages, setErrorMessages] = useState([]);
  const navigate = useNavigate();

  const signUp = async (event) => {
    event.preventDefault();

    var { login, name, password, passwordConfirm } = document.forms[0];

    const requestOptions = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        login: login.value,
        name: name.value,
        password: password.value,
        passwordConfirm: passwordConfirm.value,
      }),
    };
    return await fetch("api/Account/SignUp", requestOptions)
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
          <h3>SignUp</h3>
          <form onSubmit={signUp}>
            <label>Login </label>
            <input type="text" name="login" placeholder="Login" />
            <br />
            <label>Name </label>
            <input type="text" name="name" placeholder="Name" />
            <br />
            <label>Password </label>
            <input type="text" name="password" placeholder="Password" />
            <br />
            <label>Confirm password </label>
            <input
              type="text"
              name="passwordConfirm"
              placeholder="Confirm password"
            />
            <br />
            <button type="submit">SignUp</button>
          </form>
          {renderErrorMessage()}
        </>
      )}
    </>
  );
};

export default SignUp;
