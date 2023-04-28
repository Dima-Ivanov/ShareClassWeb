import React from "react";
import { useNavigate } from "react-router-dom";

const SignOut = ({ setUser }) => {
  const navigate = useNavigate();

  const signOut = async (event) => {
    event.preventDefault();

    const requestOptions = {
      method: "POST",
    };
    return await fetch("api/Account/SignOut", requestOptions).then(
      (response) => {
        response.status === 200 &&
          setUser({ isAuthenticated: false, userName: "" });

        if (response.status == 401) navigate("/SignIn");
      }
    );
  };

  return (
    <>
      <p></p>
      <form onSubmit={signOut}>
        <button type="submit">SignOut</button>
      </form>
    </>
  );
};

export default SignOut;
