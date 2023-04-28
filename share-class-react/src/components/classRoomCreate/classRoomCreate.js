import React from "react";

const ClassRoomCreate = ({ user, addClassRoom }) => {
  const handleSubmit = (event) => {
    event.preventDefault();
    const name = event.target.elements.name.value;
    const description = event.target.elements.description.value;
    const teacher_Name = event.target.elements.teacher_Name.value;

    const classRoom = {
      name: name,
      description: description,
      teacher_Name: teacher_Name,
    };

    const createClassRoom = async () => {
      const requestOptions = {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(classRoom),
      };

      const response = await fetch("api/ClassRooms", requestOptions);

      return await response.json().then(
        (data) => {
          console.log("Data: ", data);

          if (response.ok) {
            addClassRoom(data);

            event.target.elements.name.value = "";
            event.target.elements.description.value = "";
            event.target.elements.teacher_Name.value = "";
          }
        },
        (error) => console.log(error)
      );
    };
    createClassRoom();
  };
  return (
    <React.Fragment>
      {user.isAuthenticated ? (
        <>
          <h3>Создание класс-рума</h3>
          <form onSubmit={handleSubmit}>
            <label>Название: </label>
            <input type="text" name="name" placeholder="Введите название:" />
            <br />
            <label>Описание: </label>
            <input
              type="text"
              name="description"
              placeholder="Введите описание:"
            />{" "}
            <br />
            <label>Имя преподавателя: </label>
            <input
              type="text"
              name="teacher_Name"
              placeholder="Введите имя преподавателя:"
            />
            <br /> <br />
            <button type="submit">Создать</button>
          </form>
        </>
      ) : (
        <>
          <br />
          <span>SignIn to create classrooms</span>
        </>
      )}
    </React.Fragment>
  );
};
export default ClassRoomCreate;
