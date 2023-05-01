import React, { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import "./style.css";
import { Constants } from "../../constants/constants";
import trashIcon from "../../icons/trash.svg";
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
  DatePicker,
} from "antd";

const HomeTask = ({ user, setHeaderPlusButton }) => {
  const [homeTasks, setHomeTasks] = useState([]);
  const [classRoomAdministratorId, setClassRoomAdministratorId] = useState(-1);
  const params = useParams();
  const classRoomId = params.classRoomId;

  const removeHomeTask = (removeId) => {
    setHomeTasks((prevHomeTasks) =>
      prevHomeTasks.filter(({ id }) => id !== removeId)
    );
  };
  const addHomeTask = (homeTask) => {
    setHomeTasks((prevHomeTasks) => [...prevHomeTasks, homeTask]);
  };

  const [isCreateModalVisible, setIsCreateModalVisible] = useState(false);
  const [createHomeTaskForm] = Form.useForm();
  const [isDeleteConfirmOpen, setIsDeleteConfirmOpen] = useState({});

  useEffect(() => {
    const getHomeTasks = async () => {
      return await fetch(`/api/HomeTasks/${classRoomId}`, {
        method: "GET",
      })
        .then((response) => response.json())
        .then(
          (data) => {
            console.log("Data: ", data);
            setHomeTasks(data);
          },
          (error) => {
            console.log(error);
          }
        );
    };
    getHomeTasks();
  }, [setHomeTasks]);

  useEffect(() => {
    const getClassRoomAdministratorId = async () => {
      return await fetch(`/api/ClassRooms/${classRoomId}`, {
        method: "GET",
      })
        .then((response) => response.json())
        .then(
          (data) => {
            console.log("Data: ", data);
            setClassRoomAdministratorId(data.administrator_ID);
          },
          (error) => {
            console.log(error);
          }
        );
    };
    getClassRoomAdministratorId();
  }, [setClassRoomAdministratorId]);

  useEffect(() => {
    if (
      classRoomAdministratorId == user.userId ||
      user.userRole == Constants.adminRole
    ) {
      setHeaderPlusButton({ button: addHomeTaskMenu });
    } else {
      setHeaderPlusButton({ button: <div></div> });
    }
  }, [setHeaderPlusButton, user]);

  const deleteHomeTask = async (homeTaskId) => {
    const requestOptions = {
      method: "DELETE",
    };

    return await fetch(
      `/api/HomeTasks/${classRoomId}/${homeTaskId}`,
      requestOptions
    ).then(
      (response) => {
        if (response.ok) {
          removeHomeTask(homeTaskId);
        }
      },
      (error) => console.log(error)
    );
  };

  const addHomeTaskOption = (
    <Menu>
      <Menu.Item>
        <Button
          type="primary"
          style={{ width: "100%" }}
          onClick={() => createHomeTask()}
        >
          Create HomeTask
        </Button>
      </Menu.Item>
    </Menu>
  );

  const createHomeTask = async () => {
    setIsCreateModalVisible(true);
  };

  const handleCreateHomeTask = async () => {
    try {
      const values = await createHomeTaskForm.validateFields();

      const homeTask = {
        name: values.name,
        description: values.description,
        deadline_Date: values.deadline_Date,
        classRoom: {
          id: classRoomId, // will be changed in ASP Controller
          name: "will be changed in ASP Controller",
          description: "will be changed in ASP Controller",
          teacher_Name: "will be changed in ASP Controller",
        },
      };

      const requestOptions = {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(homeTask),
      };

      const response = await fetch(
        `/api/HomeTasks/${classRoomId}`,
        requestOptions
      );

      await response.json().then(
        (data) => {
          console.log("Data: ", data);

          if (response.ok) {
            addHomeTask(data);
            notification.info({
              message: "Success",
              description: "Created HomeTask: " + data.name,
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
      createHomeTaskForm.resetFields();
    } catch (error) {
      console.log(error);
    }
  };

  const handleCancelCreateHomeTask = () => {
    setIsCreateModalVisible(false);
  };

  const createHomeTaskModal = (
    <Modal
      title="Create HomeTask"
      open={isCreateModalVisible}
      onOk={handleCreateHomeTask}
      onCancel={handleCancelCreateHomeTask}
    >
      <Form form={createHomeTaskForm}>
        <Form.Item
          name="name"
          label="HomeTask Name"
          rules={[
            {
              required: true,
              message: "Please input the name of the HomeTask!",
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
          <Input.TextArea autoSize={{ minRows: 3, maxRows: 6 }} />
        </Form.Item>

        <Form.Item
          name="deadline_Date"
          label="Deadline Date"
          rules={[
            {
              required: true,
              message: "Please input the deadline date!",
            },
          ]}
        >
          <DatePicker
            disabledDate={(current) =>
              current && current < new Date(new Date().setHours(0, 0, 0, 0))
            }
          />
        </Form.Item>
      </Form>
    </Modal>
  );

  const addHomeTaskMenu = (
    <div className="addHomeTaskDropDown">
      <Dropdown
        overlay={addHomeTaskOption}
        trigger={["click"]}
        placement="bottomCenter"
      >
        <Button className="addHomeTaskMenu" title="Add HomeTask">
          <img src={plusIcon} alt="Add HomeTask"></img>
        </Button>
      </Dropdown>
    </div>
  );

  return (
    <React.Fragment>
      {createHomeTaskModal}
      <div className="mainContainer">
        <div className="homeTasksContainer">
          {homeTasks.map(({ id, name, deadline_Date }) => (
            <div className="homeTask" key={id} id={id}>
              <div>
                <Link
                  to={`/Solution/${classRoomId}/${id}`}
                  className="homeTaskName"
                >
                  <strong
                    style={{
                      maxWidth: "100%",
                      height: "auto",
                      overflow: "hidden",
                      whiteSpace: "nowrap",
                      textOverflow: "ellipsis",
                      display: "inline-block",
                    }}
                  >
                    {name}
                  </strong>
                </Link>
                <p className="deadlineDate">
                  Deadline date: {new Date(deadline_Date).toLocaleDateString()}
                </p>
              </div>

              <div className="homeTaskButtons">
                {user.isAuthenticated &&
                (user.userRole == Constants.adminRole ||
                  user.userId == classRoomAdministratorId) ? (
                  <Popconfirm
                    title="Are you sure you want to delete this hometask?"
                    open={isDeleteConfirmOpen[id]}
                    onConfirm={() => {
                      deleteHomeTask(id);
                      setIsDeleteConfirmOpen((prev) => ({
                        ...prev,
                        [id]: false,
                      }));
                    }}
                    onCancel={() =>
                      setIsDeleteConfirmOpen((prev) => ({
                        ...prev,
                        [id]: false,
                      }))
                    }
                    okText="Yes"
                    cancelText="No"
                  >
                    <button
                      className="deleteHomeTaskButton"
                      title="Delete HomeTask"
                      onClick={() =>
                        setIsDeleteConfirmOpen((prev) => ({
                          ...prev,
                          [id]: true,
                        }))
                      }
                    >
                      <img src={trashIcon} alt="Delete HomeTask"></img>
                    </button>
                  </Popconfirm>
                ) : (
                  ""
                )}
              </div>
            </div>
          ))}
        </div>
      </div>
    </React.Fragment>
  );
};

export default HomeTask;
