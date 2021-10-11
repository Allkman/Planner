import React from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormikControl from '../Components/FormikControl'
import {Link} from 'react-router-dom'

function LoginForm() {
  const initialValues = {
    email: "",
    password: "",
  };
  const validationSchema = Yup.object({
    email: Yup.string()
      .email("Invalid email format.")
      .required("Email is required."),
    password: Yup.string().required("Password is required."),
  });
  const onSubmit = (values) => {
    console.log("Form data", values);
  };
  return (
    <Formik
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={onSubmit}
    >
      {(formik) => {
        return (
          <Form>
            <h3>Login</h3>
            <FormikControl
              control="input"
              type="email"
              label="Email"
              name="email"
            />
            <FormikControl
              control="input"
              type="password"
              label="Password"
              name="password"
            />
            <button type="submit" >
              Register
            </button>
            <button type="submit" disabled={!formik.isValid} >
              Login
            </button>
          </Form>
        );
      }}
    </Formik>
  );
}

export default LoginForm
