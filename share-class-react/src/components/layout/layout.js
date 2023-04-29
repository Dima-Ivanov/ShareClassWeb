import React from "react";
import { Outlet, Link } from "react-router-dom";
import { Layout as LayoutAntd, Menu } from "antd";
const { Header, Content, Footer } = LayoutAntd;

const items = [
  {
    label: <Link to={"/"}>Главная</Link>,
    key: "1",
  },
  {
    label: <Link to="/ClassRooms">ClassRooms</Link>,
    key: "2",
  },
  {
    label: <Link to="/SignIn">SignIn</Link>,
    key: "3",
  },
  {
    label: <Link to="/SignUp">SignUp</Link>,
    key: "4",
  },
  {
    label: <Link to="/SignOut">SignOut</Link>,
    key: "5",
  },
];

const Layout = ({ user }) => {
  return (
    <LayoutAntd
      style={{ display: "flex", flexDirection: "column", minHeight: "98vh" }}
    >
      <Header style={{ position: "sticky", top: 0, zIndex: 1, width: "100%" }}>
        <div
          style={{
            float: "right",
            color: "rgba(255, 255, 255, 0.65)",
          }}
        >
          {user.isAuthenticated ? (
            <strong>{user.userName}</strong>
          ) : (
            <strong>Гость</strong>
          )}
        </div>
        <Menu theme="dark" mode="horizontal" items={items} className="menu" />
      </Header>
      <Content
        className="site-layout"
        style={{ padding: "0 50px", flexGrow: 1 }}
      >
        <Outlet />
      </Content>
      <Footer
        style={{
          position: "sticky",
          bottom: 0,
          zIndex: 1,
          textAlign: "center",
          width: "100%",
        }}
      >
        ShareClassWeb ©2023
      </Footer>
    </LayoutAntd>
  );
};

export default Layout;
