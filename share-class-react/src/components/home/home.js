import React from "react";

const Home = ({ setHeaderPlusButton }) => {
  React.useEffect(() => {
    setHeaderPlusButton({ button: <div></div> });
  }, [setHeaderPlusButton]);

  return (
    <>
      <h1>Home</h1>
      <div style={{ fontSize: "large" }}>
        ShareClass is a platform that provides an alternative to Classroom
        Google, where the main goal is to facilitate the exchange of homework
        solutions between students. The users are able to create class-rooms and
        manage them, add homework assignments with a title/description/due date,
        and publish/view solutions to the homework assignments. <br />
        The class-rooms can be joined by invitation only. Only the
        administrators, who are the creators of the class-rooms, are able to add
        homework assignments, while all members of the class-room can add
        solutions to the homework assignments.
        <br /> <br />
        Current version: 1.0
      </div>
    </>
  );
};

export default Home;
