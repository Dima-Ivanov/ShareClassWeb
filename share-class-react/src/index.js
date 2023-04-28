import React, { useState, useEffect } from "react";
import ReactDOM from "react-dom/client";
import ClassRoom from "./components/classRoom/classRoom";
import ClassRoomCreate from "./components/classRoomCreate/classRoomCreate";
import Layout from "./components/layout/layout";
import SignIn from "./components/signIn/signIn";
import SignOut from "./components/signOut/signOut";
import SignUp from "./components/signUp/signUp";
import { BrowserRouter, Route, Routes } from "react-router-dom";

const App = () => {
  const [classRooms, setClassRooms] = useState([]);
  const addClassRoom = (classRoom) => setClassRooms([...classRooms], classRoom);
  const removeClassRoom = (removeId) =>
    setClassRooms(
      classRooms.filter(({ classRoomId }) => classRoomId !== removeId)
    );
  const [user, setUser] = useState({ isAuthenticated: false, userName: "" });

  useEffect(() => {
    const getUser = async () => {
      return await fetch("api/Account/IsAuthenticated")
        .then((response) => {
          response.status === 401 &&
            setUser({ isAuthenticated: false, userName: "" });
          return response.json();
        })
        .then(
          (data) => {
            if (
              typeof data !== "undefined" &&
              typeof data.userName !== "undefined"
            ) {
              setUser({ isAuthenticated: true, userName: data.userName });
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
        <Route path="/" element={<Layout user={user} />}>
          <Route index element={<h3>Home</h3>} />
          <Route
            path="/ClassRooms"
            element={
              <>
                <ClassRoomCreate user={user} addClassRoom={addClassRoom} />
                <ClassRoom
                  user={user}
                  classRooms={classRooms}
                  setClassRooms={setClassRooms}
                  removeClassRoom={removeClassRoom}
                />
              </>
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
          <Route path="/SignOut" element={<SignOut setUser={setUser} />} />
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
