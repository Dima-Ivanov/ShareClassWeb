import React, { useEffect } from "react";
import "./style.css";

const ClassRoom = ({ classRooms, setClassRooms, removeClassRoom }) => {
  useEffect(() => {
    const getClassRooms = async () => {
      return await fetch("https://localhost:7088/api/ClassRooms", {
        method: "GET",
      })
        .then((response) => response.json())
        .then(
          (data) => {
            console.log("Data: ", data);
            setClassRooms(data);
          },
          (error) => {
            console.log(error);
          }
        );
    };
    getClassRooms();
  }, [setClassRooms]);

  const removeItem = async (classRoomId) => {
    const requestOptions = {
      method: "DELETE",
    };

    return await fetch(
      `https://localhost:7088/api/ClassRooms/${classRoomId}`,
      requestOptions
    ).then(
      (response) => {
        if (response.ok) {
          removeClassRoom(classRoomId);
        }
      },
      (error) => console.log(error)
    );
  };

  return (
    <React.Fragment>
      <h3>Список класс-румов</h3>
      {classRooms.map(
        ({
          id,
          name,
          invitationCode,
          description,
          teacher_Name,
          students_Count,
          creation_Date,
          homeTask,
        }) => (
          <div className="ClassRoom" key={id} id={id}>
            {" "}
            <br />
            <strong className="ClassRoomName">{name} &emsp;</strong>
            <button onClick={(e) => removeItem(id)}>Удалить</button>
            <br />
            <span>Пригласительный код: {invitationCode}</span> <br />
            <span>Описание: {description}</span> <br />
            <span>Имя преподавателя: {teacher_Name}</span> <br />
            <span>Кол-во студентов: {students_Count}</span> <br />
            <span>Дата создания: {creation_Date}</span> <br />
            {homeTask.map(({ id, name, description }) => (
              <div className="HomeTask" key={id} id={id}>
                <span>Название дз: {name}</span> <br />
                <span>Описание: {description}</span> <br />
              </div>
            ))}
          </div>
        )
      )}
    </React.Fragment>
  );
};

export default ClassRoom;
