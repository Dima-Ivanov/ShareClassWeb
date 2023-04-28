import React from "react";
import { Outlet, Link } from "react-router-dom";

const Layout = ({ user }) => {
  return (
    <>
      <div>
        <h4>
          User:&ensp;
          {user.isAuthenticated ? user.userName : "Guest"}
        </h4>
      </div>

      <nav>
        <Link to="/">Home</Link> <br />
        <Link to="/ClassRooms">ClassRooms</Link> <br />
        <Link to="/SignIn">SignIn</Link> <br />
        <Link to="/SignUp">SignUp</Link> <br />
        <Link to="/SignOut">SignOut</Link>
      </nav>
      <Outlet />
    </>
  );
};

export default Layout;
