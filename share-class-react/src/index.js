import React, { useState, useEffect } from "react";
import ReactDOM from "react-dom/client";
import ClassRoom from "./components/classRoom/classRoom";
import Layout from "./components/layout/layout";
import SignIn from "./components/signIn/signIn";
import SignUp from "./components/signUp/signUp";
import Home from "./components/home/home";
import { BrowserRouter, Route, Routes } from "react-router-dom";

const App = () => {
  const [classRooms, setClassRooms] = useState([]);
  const removeClassRoom = (removeId) =>
    setClassRooms(
      classRooms.filter(({ classRoomId }) => classRoomId !== removeId)
    );
  const [user, setUser] = useState({
    isAuthenticated: false,
    userName: "",
    userRole: "",
  });

  const [headerPlusButton, setHeaderPlusButton] = useState({
    button: "",
  });

  useEffect(() => {
    const getUser = async () => {
      return await fetch("api/Account/IsAuthenticated")
        .then((response) => {
          response.status === 401 &&
            setUser({ isAuthenticated: false, userName: "", userRole: "" });
          return response.json();
        })
        .then(
          (data) => {
            if (
              typeof data !== "undefined" &&
              typeof data.userName !== "undefined"
            ) {
              setUser({
                isAuthenticated: true,
                userName: data.userName,
                userRole: data.userRole,
              });
            }
          },
          (error) => {
            console.log(error);
          }
        );
    };
    getUser();
  }, [setUser]);

  return (
    <BrowserRouter>
      <Routes>
        <Route
          path="/"
          element={
            <Layout
              user={user}
              setUser={setUser}
              headerPlusButton={headerPlusButton}
            />
          }
        >
          <Route
            index
            element={<Home setHeaderPlusButton={setHeaderPlusButton} />}
          />
          <Route
            path="/ClassRooms"
            element={
              <ClassRoom
                user={user}
                classRooms={classRooms}
                setClassRooms={setClassRooms}
                removeClassRoom={removeClassRoom}
                setHeaderPlusButton={setHeaderPlusButton}
              />
            }
          />
          <Route
            path="/SignIn"
            element={<SignIn user={user} setUser={setUser} />}
          />
          <Route
            path="/SignUp"
            element={<SignUp user={user} setUser={setUser} />}
          />
          <Route path="*" element={<h3>404</h3>} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
};

const root = ReactDOM.createRoot(document.getElementById("root"));

root.render(
  // <React.StrictMode>
  <App />
  // </React.StrictMode>
);
