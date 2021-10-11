import React from 'react';
import {useFormik} from "formik";
import * as Yup from 'yup';

const initialValues = { title: ''}

const onSubmit = values => {
  console.log('Form data', values)
}
const validate = values => {
  let errors = {}
    if(!values.title) {
      errors.title = 'Required'
    }
  return errors
}

const validationSchema = Yup.object({
  title: Yup.string().required('Title is required.')
})

function OldProjectForm() {
//useFormik hook to return an object stored in formik variable
  const formik = useFormik({
    initialValues,
    onSubmit,
    validationSchema
    //validate
  })
console.log('Visited fields: ', formik.touched)
  return (
    <div className='form-control'>
      <form onSubmit={formik.handleSubmit}>
        <div>
        <label htmlFor='title'>Project Title</label>
                                          {/*title is used in initialValues {} */}
        <input 
        type='text'
        id='title' 
        name='title' 
        onChange={formik.handleChange} 
        //formik method to check if this field is clicked on (visited)
        onBlur={formik.handleBlur}
        value={formik.values.title}
        />
        {/* only if the field has been visited and the error message exists, then show the message */}
        { formik.touched.title && formik.errors.title ? 
        <div className='error'>{formik.errors.title}</div> : null}
        </div>
        <button type='submit'>Submit</button>
      </form>
    </div>
  )
}

export default ProjectForm;
