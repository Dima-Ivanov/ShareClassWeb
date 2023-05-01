import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
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
  Collapse,
} from "antd";

const Solution = ({ user, setHeaderPlusButton }) => {
  const [solutions, setSolutions] = useState([]);
  const [classRoomAdministratorId, setClassRoomAdministratorId] = useState(-1);
  const params = useParams();
  const classRoomId = params.classRoomId;
  const homeTaskId = params.homeTaskId;
  const [homeTask, setHomeTask] = useState({
    id: homeTaskId,
    name: "",
    description: "",
    creation_Date: "",
    deadline_Date: "",
  });

  const removeSolution = (removeId) => {
    console.log(solutions);
    setSolutions((prevSolutions) =>
      prevSolutions.filter(({ solution }) => solution.id !== removeId)
    );
    console.log(solutions);
  };
  const addSolution = (solution) => {
    setSolutions((prevSolutions) => [...prevSolutions, solution]);
  };

  const [isCreateModalVisible, setIsCreateModalVisible] = useState(false);
  const [createSolutionForm] = Form.useForm();
  const [isDeleteConfirmOpen, setIsDeleteConfirmOpen] = useState({});

  useEffect(() => {
    const getSolutions = async () => {
      return await fetch(`/api/Solutions/${classRoomId}/${homeTaskId}`, {
        method: "GET",
      })
        .then((response) => response.json())
        .then(
          (data) => {
            console.log("Data: ", data);
            setSolutions(data);
          },
          (error) => {
            console.log(error);
          }
        );
    };
    getSolutions();
  }, [setSolutions]);

  useEffect(() => {
    const getHomeTask = async () => {
      return await fetch(`/api/HomeTasks/${classRoomId}/${homeTaskId}`, {
        method: "GET",
      })
        .then((response) => response.json())
        .then(
          (data) => {
            console.log("Data: ", data);
            setHomeTask({
              id: homeTaskId,
              name: data.name,
              description: data.description,
              creation_Date: data.creation_Date,
              deadline_Date: data.deadline_Date,
            });
          },
          (error) => {
            console.log(error);
          }
        );
    };
    getHomeTask();
  }, [setHomeTask]);

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
    setHeaderPlusButton({ button: addSolutionMenu });
  }, [setHeaderPlusButton]);

  const deleteSolution = async (solutionId) => {
    const requestOptions = {
      method: "DELETE",
    };

    return await fetch(
      `/api/Solutions/${classRoomId}/${homeTaskId}/${solutionId}`,
      requestOptions
    ).then(
      (response) => {
        if (response.ok) {
          removeSolution(solutionId);
        }
      },
      (error) => console.log(error)
    );
  };

  const addSolutionOption = (
    <Menu>
      <Menu.Item>
        <Button
          type="primary"
          style={{ width: "100%" }}
          onClick={() => createSolution()}
        >
          Create Solution
        </Button>
      </Menu.Item>
    </Menu>
  );

  const createSolution = async () => {
    setIsCreateModalVisible(true);
  };

  const handleCreateSolution = async () => {
    try {
      const values = await createSolutionForm.validateFields();

      const solution = {
        solution_Text: values.solution_Text,
        userID: 0,
      };

      const requestOptions = {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(solution),
      };

      const response = await fetch(
        `/api/Solutions/${classRoomId}/${homeTaskId}`,
        requestOptions
      );

      await response.json().then(
        (data) => {
          console.log("Data: ", data);

          if (response.ok) {
            addSolution(data);
            notification.info({
              message: "Success",
              description: "Created Solution",
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
      createSolutionForm.resetFields();
    } catch (error) {
      console.log(error);
    }
  };

  const handleCancelCreateSolution = () => {
    setIsCreateModalVisible(false);
  };

  const createSolutionModal = (
    <Modal
      title="Create Solution"
      open={isCreateModalVisible}
      onOk={handleCreateSolution}
      onCancel={handleCancelCreateSolution}
      className="createSolutionModal"
    >
      <Form
        form={createSolutionForm}
        style={{ width: "47.5rem", height: "30vh", minHeight: "30vh" }}
      >
        <Form.Item
          name="solution_Text"
          label="Solution Text"
          rules={[
            {
              required: true,
              message: "Please input the solution text!",
            },
          ]}
        >
          <Input.TextArea
            style={{ height: "100%" }}
            autoSize={{ minRows: 5, maxRows: 10 }}
          />
        </Form.Item>
      </Form>
    </Modal>
  );

  const addSolutionMenu = (
    <div className="addSolutionDropDown">
      <Dropdown
        overlay={addSolutionOption}
        trigger={["click"]}
        placement="bottomCenter"
      >
        <Button className="addSolutionMenu" title="Add Solution">
          <img src={plusIcon} alt="Add Solution"></img>
        </Button>
      </Dropdown>
    </div>
  );

  const horizontalDivider = (
    <hr
      style={{
        width: "100%",
        border: "none",
        borderTop: "1px solid black",
      }}
    />
  );

  return (
    <React.Fragment>
      {createSolutionModal}
      <div className="mainSolutionContainer">
        <div className="homeTaskContainer">
          <h1
            style={{
              overflow: "hidden",
              textOverflow: "ellipsis",
              maxWidth: "100%",
              wordWrap: "break-word",
            }}
          >
            {homeTask.name}
          </h1>
          {horizontalDivider}
          <h3
            style={{
              width: "100%",
              height: "auto",
              overflow: "hidden",
              whiteSpace: "pre-wrap",
              textOverflow: "ellipsis",
              wordWrap: "break-word",
            }}
          >
            {homeTask.description}
          </h3>
          {horizontalDivider}
          <span style={{ fontWeight: "bold", fontSize: "17px" }}>
            Creation date:{" "}
            {new Date(homeTask.creation_Date).toLocaleDateString()}
          </span>
          <span style={{ fontWeight: "bold", fontSize: "17px" }}>
            Deadline date:{" "}
            {new Date(homeTask.creation_Date).toLocaleDateString()}
          </span>
          {horizontalDivider}
        </div>
        <div className="solutionsWrapper">
          <h2>Solutions:</h2>
          <div className="solutionsContainer">
            {solutions.map(({ solution, userName }) => (
              <div
                className="solution"
                key={solution.id}
                id={solution.id}
                style={{ width: "100%" }}
              >
                <div className="solutionHeader">
                  <div>Author: {userName}</div>

                  <div className="solutionButtons">
                    {user.isAuthenticated &&
                    (user.userRole == Constants.adminRole ||
                      user.userId == classRoomAdministratorId ||
                      user.userId == solution.userID) ? (
                      <Popconfirm
                        title="Are you sure you want to delete this solution?"
                        open={isDeleteConfirmOpen[solution.id]}
                        onConfirm={() => {
                          deleteSolution(solution.id);
                          setIsDeleteConfirmOpen((prev) => ({
                            ...prev,
                            [solution.id]: false,
                          }));
                        }}
                        onCancel={() =>
                          setIsDeleteConfirmOpen((prev) => ({
                            ...prev,
                            [solution.id]: false,
                          }))
                        }
                        okText="Yes"
                        cancelText="No"
                      >
                        <button
                          className="deleteSolutionButton"
                          title="Delete Solution"
                          onClick={() =>
                            setIsDeleteConfirmOpen((prev) => ({
                              ...prev,
                              [solution.id]: true,
                            }))
                          }
                        >
                          <img src={trashIcon} alt="Delete Solution"></img>
                        </button>
                      </Popconfirm>
                    ) : (
                      ""
                    )}
                  </div>
                </div>

                <div className="solutionBody">
                  <Collapse>
                    <Collapse.Panel header="Solution Text" key="1">
                      {solution.solution_Text}
                    </Collapse.Panel>
                  </Collapse>
                </div>

                {horizontalDivider}
              </div>
            ))}
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

export default Solution;
