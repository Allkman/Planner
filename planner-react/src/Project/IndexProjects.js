import React from 'react'; 
import axios from "axios";
import ProjectTableRow from './ProjectTableRow';
import { urlProjects } from '../endpoints';

// const api  = axios.get({
//    baseURL: `https://localhost:44303/api/projects/`
// })

class IndexProjects extends React.Component {  
 constructor(props) {  
      super(props);  
      this.state = {
         projects: []
      }      
   }  
   componentDidMount(){
      const _this = this;

      axios.get(urlProjects).then(function(response) {
         console.log(response)
      })      
   }
   render() {  
      return (  
         <div>  
            <button >Add Project</button>
            <table>
               <thead>
                  <tr>
                     <th>#</th>
                     <th>Project</th>
                  </tr>
               </thead>
               <tbody>
                  {
                  this.state.projects.map(function(user) 
                  { return <ProjectTableRow user={user} />})}          
               </tbody>
            </table>
         </div>  
      );  
   }  
}  
export default IndexProjects;