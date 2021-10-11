import axios from "axios";
import ProjectForm from "./ProjectForm";
import { useHistory } from 'react-router-dom';
import React from 'react'
import { urlProjects } from "../endpoints";

function CreateProject() {
  const history = useHistory();

  async function create(
    name){
    
        await axios.post(urlProjects, name);
        history.push('/projects');
    }
  



  return (
    <>
    <h3>Create a Project</h3>
    <ProjectForm onSubmit={async value => { await create(value)}}/>
    </>
  )
}

export default CreateProject
