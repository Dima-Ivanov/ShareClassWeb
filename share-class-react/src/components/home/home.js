import React from "react";

const Home = ({ setHeaderPlusButton }) => {
  React.useEffect(() => {
    setHeaderPlusButton({ button: <div></div> });
  }, [setHeaderPlusButton]);

  return (
    <>
      <h3>Home</h3>
    </>
  );
};

export default Home;
