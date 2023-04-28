import React, { useState } from "react";
import ReactDOM from "react-dom/client";
import ClassRoom from "./components/classRoom/classRoom";
import ClassRoomCreate from "./components/classRoomCreate/classRoomCreate";

const App = () => {
  const [classRooms, setClassRooms] = useState([]);
  const addClassRoom = (classRoom) => setClassRooms([...classRooms], classRoom);
  const removeClassRoom = (removeId) =>
    setClassRooms(
      classRooms.filter(({ classRoomId }) => classRoomId !== removeId)
    );
  return (
    <div>
      <ClassRoomCreate addClassRoom={addClassRoom} />
      <ClassRoom
        classRooms={classRooms}
        setClassRooms={setClassRooms}
        removeClassRoom={removeClassRoom}
      />
    </div>
  );
};

const root = ReactDOM.createRoot(document.getElementById("root"));

root.render(
  // <React.StrictMode>
  <App />
  // </React.StrictMode>
);
