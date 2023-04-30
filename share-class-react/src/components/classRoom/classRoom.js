import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import "./style.css";
import { Constants } from "../../constants/constants";
import trashIcon from "../../icons/trash.svg";
import copyIcon from "../../icons/copy.svg";
import exitIcon from "../../icons/exit.svg";
import plusIcon from "../../icons/plus.svg";
import {
  Button,
  Dropdown,
  Menu,
  Form,
  Modal,
  Input,
  notification,
  Popconfirm,
} from "antd";

const ClassRoom = ({
  user,
  classRooms,
  setClassRooms,
  removeClassRoom,
  addClassRoom,
  setHeaderPlusButton,
}) => {
  const [isCreateModalVisible, setIsCreateModalVisible] = useState(false);
  const [isJoinModalVisible, setIsJoinModalVisible] = useState(false);
  const [createClassRoomForm] = Form.useForm();
  const [joinClassRoomForm] = Form.useForm();
  const [isDeleteConfirmOpen, setIsDeleteConfirmOpen] = useState(false);

  useEffect(() => {
    const getClassRooms = async () => {
      return await fetch("api/ClassRooms", {
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

  useEffect(() => {
    setHeaderPlusButton({ button: addOrJoinClassRoomMenu });
  }, [setHeaderPlusButton]);

  const deleteClassRoom = async (classRoomId) => {
    const requestOptions = {
      method: "DELETE",
    };

    return await fetch(`api/ClassRooms/${classRoomId}`, requestOptions).then(
      (response) => {
        if (response.ok) {
          removeClassRoom(classRoomId);
        }
      },
      (error) => console.log(error)
    );
  };

  const copyInvitationCode = async (invitationCode) => {
    await navigator.clipboard.writeText(invitationCode);
    notification.open({
      message: "Invitation code copied!",
      description: invitationCode,
      duration: 1,
    });
  };

  const leaveClassRoom = async (classRoomId) => {
    const requestOptions = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
    };

    const response = await fetch(
      `api/ClassRooms/Leave/${classRoomId}`,
      requestOptions
    );

    await response.json().then(
      (data) => {
        console.log("Data: ", data);

        if (response.ok) {
          removeClassRoom(data);
          notification.info({
            message: "Success",
            description: "Left ClassRoom: " + data.name,
            duration: 2,
          });
        } else {
          notification.error({
            message: "Error",
            description: data.message,
          });
        }
      },
      (error) => console.log(error)
    );
  };

  const addOrJoinClassRoomOptions = (
    <Menu>
      <Menu.Item>
        <Button
          type="primary"
          style={{ width: "100%" }}
          onClick={() => createClassRoom()}
        >
          Create ClassRoom
        </Button>
      </Menu.Item>
      <Menu.Item>
        <Button
          type="primary"
          style={{ width: "100%" }}
          onClick={() => joinClassRoom()}
        >
          Join ClassRoom
        </Button>
      </Menu.Item>
    </Menu>
  );

  const createClassRoom = async () => {
    setIsCreateModalVisible(true);
  };

  const handleCreateClassRoom = async () => {
    try {
      const values = await createClassRoomForm.validateFields();

      const classRoom = {
        name: values.name,
        description: values.description,
        teacher_Name: values.teacher_Name,
      };

      const requestOptions = {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(classRoom),
      };

      const response = await fetch("api/ClassRooms", requestOptions);

      await response.json().then(
        (data) => {
          console.log("Data: ", data);

          if (response.ok) {
            addClassRoom(data);
            notification.info({
              message: "Success",
              description: "Created ClassRoom: " + data.name,
              duration: 2,
            });
          } else {
            notification.error({
              message: "Error",
              description: data.message,
            });
          }
        },
        (error) => console.log(error)
      );

      setIsCreateModalVisible(false);
      createClassRoomForm.resetFields();
    } catch (error) {
      console.log(error);
    }
  };

  const handleCancelCreateClassRoom = () => {
    setIsCreateModalVisible(false);
  };

  const createClassRoomModal = (
    <Modal
      title="Create ClassRoom"
      open={isCreateModalVisible}
      onOk={handleCreateClassRoom}
      onCancel={handleCancelCreateClassRoom}
    >
      <Form form={createClassRoomForm}>
        <Form.Item
          name="name"
          label="ClassRoom Name"
          rules={[
            {
              required: true,
              message: "Please input the name of the ClassRoom!",
            },
          ]}
        >
          <Input />
        </Form.Item>

        <Form.Item
          name="description"
          label="Description"
          rules={[
            {
              required: true,
              message: "Please input the description!",
            },
          ]}
        >
          <Input />
        </Form.Item>

        <Form.Item
          name="teacher_Name"
          label="Teacher Name"
          rules={[
            {
              required: true,
              message: "Please input the teacher name!",
            },
          ]}
        >
          <Input />
        </Form.Item>
      </Form>
    </Modal>
  );

  const handleJoinClassRoom = async () => {
    try {
      const values = await joinClassRoomForm.validateFields();

      const requestOptions = {
        method: "POST",
        headers: { "Content-Type": "application/json" },
      };

      const response = await fetch(
        "api/ClassRooms/Join/" + values.invitationCode,
        requestOptions
      );

      await response.json().then(
        (data) => {
          console.log("Joined classroom: ", data);

          if (response.ok) {
            addClassRoom(data);
            notification.info({
              message: "Success",
              description: "Joined ClassRoom: " + data.name,
              duration: 2,
            });
          } else {
            notification.error({
              message: "Error",
              description: data.message,
            });
          }
        },
        (error) => console.log(error)
      );

      setIsJoinModalVisible(false);
      joinClassRoomForm.resetFields();
    } catch (error) {
      console.log(error);
    }
  };

  const handleCancelJoinClassRoom = () => {
    setIsJoinModalVisible(false);
  };

  const joinClassRoomModal = (
    <Modal
      title="Join ClassRoom"
      open={isJoinModalVisible}
      onOk={handleJoinClassRoom}
      onCancel={handleCancelJoinClassRoom}
    >
      <Form form={joinClassRoomForm}>
        <Form.Item
          name="invitationCode"
          label="Invitation code"
          rules={[
            {
              required: true,
              message: "Please input the invitation code!",
            },
          ]}
        >
          <Input />
        </Form.Item>
      </Form>
    </Modal>
  );

  const joinClassRoom = async () => {
    setIsJoinModalVisible(true);
  };

  const addOrJoinClassRoomMenu = (
    <div className="addOrJoinClassRoomDropDown">
      <Dropdown
        overlay={addOrJoinClassRoomOptions}
        trigger={["click"]}
        placement="bottomCenter"
      >
        <Button
          className="addOrJoinClassRoomButton"
          title="Add Or Join ClassRoom"
        >
          <img src={plusIcon} alt="Leave ClassRoom"></img>
        </Button>
      </Dropdown>
    </div>
  );

  return (
    <React.Fragment>
      {createClassRoomModal}
      {joinClassRoomModal}
      <div className="mainContainer">
        <div className="classRoomsContainer">
          {classRooms.map(
            ({ id, name, invitationCode, teacher_Name, administrator_ID }) => (
              <div className="classRoom" key={id} id={id}>
                <div>
                  <Link to={`/ClassRoom/${id}`} className="classRoomName">
                    <strong>{name}</strong>
                  </Link>
                  <p className="classRoomTeacher">{teacher_Name}</p>
                </div>

                <div className="classRoomButtons">
                  <button
                    className="copyInvitationCodeButton"
                    onClick={() => copyInvitationCode(invitationCode)}
                    title="Copy Invitation Code"
                  >
                    <img src={copyIcon} alt="Copy Invitation Code"></img>
                  </button>

                  {user.userId != administrator_ID ? (
                    <button
                      className="leaveClassRoomButton"
                      onClick={() => leaveClassRoom(id)}
                      title="Leave ClassRoom"
                    >
                      <img src={exitIcon} alt="Leave ClassRoom"></img>
                    </button>
                  ) : (
                    ""
                  )}

                  {user.isAuthenticated &&
                  (user.userRole == Constants.adminRole ||
                    user.userId == administrator_ID) ? (
                    <Popconfirm
                      title="Are you sure you want to delete this class?" // TODO показывается для всех класс-румов
                      open={isDeleteConfirmOpen}
                      onConfirm={() => {
                        deleteClassRoom(id);
                        setIsDeleteConfirmOpen(false);
                      }}
                      onCancel={() => setIsDeleteConfirmOpen(false)}
                      okText="Yes"
                      cancelText="No"
                    >
                      <button
                        className="deleteClassRoomButton"
                        onClick={() => setIsDeleteConfirmOpen(true)}
                      >
                        <img src={trashIcon} alt="Delete ClassRoom"></img>
                      </button>
                    </Popconfirm>
                  ) : (
                    ""
                  )}
                </div>
              </div>
            )
          )}
        </div>
      </div>
    </React.Fragment>
  );
};

export default ClassRoom;
