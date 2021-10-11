import React, { useState} from 'react';
import {Formik, Form, Field, ErrorMessage, FastField} from "formik";
import * as Yup from 'yup';
import TextError from './TextError';

const initialValues = { 
    name: '',
    comments: '',
    owner: '',
    reportDate: null,
    dueDate: null
  }
  
//   const savedValues = {
//     name: "Algirdas",
//     comments: "SomeSome Um Om DOM DOM",
//     owner: "Chicken street",
//   };
  const onSubmit = (values, onSubmitProps) => {
    // console.log('Form data', values)
    // console.log('Submit props', onSubmitProps)
    onSubmitProps.setSubmitting(false)
    onSubmitProps.resetForm()
  }
const validationSchema = Yup.object({
    name: Yup.string().required('Title is required.'),
    comments: Yup.string().required('Description is required'),
    reportDate: Yup.date().required(),
    dueDate: Yup.date().when("ReportDate", (reportDate, schema) =>
     reportDate && schema.min(reportDate))
  })

function StageForm() {
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
          console.log('Testing formik props on StageForm', formik)
          return (
      <Form>
      <div className="form-control">
        <label htmlFor="name">Stage Title</label>
        <FastField type="text" id="name" name="name" />
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

export default StageForm
