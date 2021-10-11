import React, { Component } from 'react';
import {projectUrl} from '../endpoints.js'
import axios from 'axios';

export default class ProjectPage extends Component {
  constructor(props) {
    super(props);
    this.state = {
      projects: [],
      modalTitle: "",
      ProjectTitle: "",
      ProjectId: 0,
    }
  }

  componentDidMount() {

  axios.get(projectUrl)
      .then(response => this.setState({ projects: response.data }));
  }

  changeProjectName = (e) => {
    this.setState({ ProjectTitle: e.target.value });
  }
  addClick() {
    this.setState({
      modalTitle: "Add Project",
      ProjectId: 0,
      ProjectTitle: ""
    });
  }
  editClick(p) {
    this.setState({
      modalTitle: "Edit Project",
      ProjectId: p.ProjectId,
      ProjectTitle: p.ProjectTitle
    });
  }
  createClick() {
    fetch(projectUrl, {
      method: "POST",
      headers: {
        Accept: "application/jason",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        ProjectTitle: this.state.ProjectTitle,
      })
    })
      .then((res) => res.json())
      .then((result) => {
          alert(result);
          this.refreshList();
        }, (error) => {
          alert("Failed to create a new project.");
        })
  }
  updateClick(){
    fetch(projectUrl,{
        method:'PUT',
        headers:{
            'Accept':'application/json',
            'Content-Type':'application/json'
        },
        body:JSON.stringify({
            ProjectId:this.state.ProjectId,
            ProjectTitle:this.state.ProjectTitle
        })
    })
    .then(res=>res.json())
    .then((result)=>{
        alert(result);
        this.refreshList();
    },(error)=>{
        alert('Failed to update a project');
    })
}

deleteClick(id){
    if(window.confirm('Are you sure?')){
    fetch(projectUrl + id,{
        method:'DELETE',
        headers:{
            'Accept':'application/json',
            'Content-Type':'application/json'
        }
    })
    .then(res=>res.json())
    .then((result)=>{
        alert(result);
        this.refreshList();
    },(error)=>{
        alert('Failed to delete a project');
    })
    }
}
  render() {
    const { projects, modalTitle, ProjectId, ProjectTitle } = this.state;
    return (
      <div>
        <button
          type="button"
          className="btn btn-primary m-2 float-end"
          data-bs-toggle="modal"
          data-bs-target="#exampleModal"
          onClick={() => this.addClick()}
        >
          Add Project
        </button>
        <table className="table table-striped">
          <thead>
            <tr>
              {/* <div className="d-flex flex-row">

</div> */}
              <th>Project Id</th>
              <th>Project Title</th>
              <th>Options</th>
            </tr>
          </thead>
          <tbody>
            {projects.map(p => 
              <tr key={p.ProjectId}>
                <td>{p.ProjectId}</td>
                <td>{p.ProjectTitle}</td>
                <td>
                  <button
                    type="button"
                    className="btn btn-light mr-1"
                    data-bs-toggle="modal"
                    data-bs-target="#exampleModal"
                    onClick={() => this.editClick(p)}
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="16"
                      height="16"
                      fill="currentColor"
                      className="bi bi-pencil-square"
                      viewBox="0 0 16 16"
                    >
                      <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
                      <path
                        fillRule="evenodd"
                        d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z"
                      />
                    </svg>
                  </button>
                  <button
                    type="button"
                    className="btn btn-light mr-1"
                    onClick={() => this.deleteClick(p.ProjectId)}
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="16"
                      height="16"
                      fill="currentColor"
                      className="bi bi-trash-fill"
                      viewBox="0 0 16 16"
                    >
                      <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z" />
                    </svg>
                  </button>
                </td>
              </tr>
            )}
          </tbody>
        </table>
        <div
          className="modal fade"
          id="exampleModal"
          tabIndex="-1"
          aria-hidden="true"
        >
          <div className="modal-dialog modal-lg modal-dialog-centered">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">{modalTitle}</h5>
                <button
                  type="button"
                  className="btn-close"
                  data-bs-dismiss="modal"
                  aria-label="Close"
                ></button>
              </div>
              <div className="modal-body">
                <div className="input-group mb-3">
                  <span className="input-group-text">Project Name</span>
                  <input
                    type="text"
                    className="form-control"
                    value={ProjectTitle}
                    onChange={this.changeProjectName}
                  />
                </div>
                {ProjectId === 0 ? 
                  <button
                    type="button"
                    className="btn btn-primary float-start"
                    onClick={() => this.createClick()}
                  >
                    Create
                  </button>
                 : null}
                {ProjectId !== 0 ? 
                  <button type="button" 
                  className="btn btn-primary float-start"
                  onClick={()=>this.updateClick()}>
                    Update
                  </button>
                 : null}
              </div>
            </div>
          </div>
        </div>
      </div>
    )
  }
}
