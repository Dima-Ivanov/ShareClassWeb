import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Modal } from "antd";

const SignOut = ({ setUser }) => {
  const [open, setOpen] = useState(false);
  const navigate = useNavigate();

  const showModal = () => {
    setOpen(true);
  };

  useEffect(() => {
    showModal();
  }, []);

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

  const handleCancel = () => {
    console.log("Clicked cancel button");
    setOpen(false);
    navigate("/");
  };

  return (
    <>
      <Modal title="Title" open={open} onOk={signOut} onCancel={handleCancel}>
        <p>Are you sure to sign out?</p>
      </Modal>
    </>
  );
};

export default SignOut;
