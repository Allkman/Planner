import React, { useState} from 'react';
import {Formik, Form, Field, ErrorMessage, FastField} from "formik";
import * as Yup from 'yup';
import TextError from '../Utils/TextError';

const initialValues = { 
  name: '',
  comments: '',
  address: ''
}

const savedValues = { 
  name: 'Algirdas',
  comments: 'SomeSome Um Om DOM DOM',
  address: 'Chicken street'
}
const onSubmit = (values, onSubmitProps) => {
  console.log('Form data', values)
  console.log('Submit props', onSubmitProps)
  onSubmitProps.setSubmitting(false)
  onSubmitProps.resetForm()
}

const validationSchema = Yup.object({
  name: Yup.string().required('Title is required.'),
  //comments: Yup.string().required('Description is required')
})

const validateComments = value => {
  let error
  if (!value) {
    error = 'Description is required'
  }
  return error
}
function ProjectForm() {
  const [formValues, setFormValues] = useState(null)
  return (
    <Formik
      initialValues={formValues || initialValues}
      validationSchema={validationSchema}
      onSubmit={onSubmit}
      enableReinitialize
      //validateOnMount
    >
      {
        formik => {
          console.log('Testing formik props on ProjectForm', formik)
          return (
      //Form automatically links above onSubmit with Form`s submit method
      <Form>
      <div className="form-control">
        <label htmlFor="name">Project Title</label>
        {/* Field will hook up inputed values to the top level Formik component /
       uses title attribute to match up with Forkik state /
       Field will render input element*/}
        <FastField type="text" id="name" name="name" />
        {/* only if the field has been visited and the error message exists, then show the message */}
        <ErrorMessage name="name" component={TextError} />
      </div>
      <div className="form-control">
        <label htmlFor="comments">Comments</label>
        <Field
          as="textarea"
          id="comments"
          name="comments"
          validate={validateComments}
        />
        <ErrorMessage name='comments' component={TextError} />
      </div>
      <div className="form-control">
        <label htmlFor="address">Address</label>
        <FastField name="address">
          {(props) => {
            const { field, form, meta} = props
            //console.log('Render props', props)
            return <div>
              <input type='text' id="address" {...field} />
              {meta.touched && meta.error ? <div>{meta.error}</div> : null}
              </div>
          }}
        </FastField>
      </div>
      {/* <button type="button" onClick={() => formik.validateField('comments')}>Validate Comments</button> */}
      <button type="button" onClick={() => setFormValues(savedValues)}>Load saved Data</button>
      <button type="reset" >Reset Data</button>
      <button type="submit" disabled={!formik.isValid || formik.isSubmitting}>Submit</button>
    </Form>
          )
        }
      }
      
    </Formik>
  );
}

export default ProjectForm;
