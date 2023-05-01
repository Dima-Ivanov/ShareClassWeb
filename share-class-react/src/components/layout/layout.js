import React from "react";
import { Outlet, Link, useNavigate } from "react-router-dom";
import { Layout as LayoutAntd, Menu, Button, Dropdown } from "antd";
import { UserOutlined, LogoutOutlined } from "@ant-design/icons";
const { Header, Content, Footer } = LayoutAntd;

const items = [
  {
    label: <Link to={"/"}>About</Link>,
    key: "1",
  },
  {
    label: <Link to="/ClassRooms">ClassRooms</Link>,
    key: "2",
  },
];

const Layout = ({ user, signOut, headerPlusButton }) => {
  console.log(headerPlusButton);

  const navigate = useNavigate();

  const handleSignOut = async () => {
    const requestOptions = {
      method: "POST",
    };

    return await fetch("/api/Account/SignOut", requestOptions).then(
      (response) => {
        response.status === 200 && signOut();

        if (response.status == 401) navigate("/SignIn");
      }
    );
  };

  const menu = user.isAuthenticated ? (
    <Menu>
      <Menu.Item onClick={handleSignOut}>
        <LogoutOutlined /> SignOut
      </Menu.Item>
    </Menu>
  ) : (
    <Menu>
      <Menu.Item>
        <Button type="link" style={{ width: "100%" }} href="/SignIn">
          SignIn
        </Button>
      </Menu.Item>
      <Menu.Item>
        <Button type="link" style={{ width: "100%" }} href="/SignUp">
          SignUp
        </Button>
      </Menu.Item>
    </Menu>
  );

  return (
    <LayoutAntd
      style={{ display: "flex", flexDirection: "column", minHeight: "98vh" }}
    >
      <Header
        style={{
          position: "sticky",
          top: 0,
          zIndex: 1,
          width: "100%",
          display: "flex",
          alignItems: "center",
          justifyContent: "space-between",
        }}
      >
        <Menu theme="dark" mode="horizontal" items={items} className="menu" />

        <div
          className="buttons"
          style={{
            display: "flex",
            alignItems: "center",
          }}
        >
          {headerPlusButton.button}

          <Dropdown overlay={menu} trigger={["click"]} placement="bottomCenter">
            <Button
              type="link"
              icon={<UserOutlined />}
              style={{ marginLeft: "auto" }}
            >
              {user.isAuthenticated ? user.userName : "Guest"}
            </Button>
          </Dropdown>
        </div>
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
