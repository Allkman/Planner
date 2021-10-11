import React from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormikControl from './FormikControl'
import { Link } from 'react-router-dom'

function RegistrationForm () {
//   const options = [
//     { key: 'Email', value: 'emailmoc' },
//     { key: 'Telephone', value: 'telephonemoc' }
//   ]
  const initialValues = {
    email: '',
    password: '',
    confirmPassword: ''
    // modeOfContact: '',
    // phone: ''
  }

  const validationSchema = Yup.object({
    email: Yup.string()
      .email('Invalid email format')
      .required('Email is required'),
    password: Yup.string().required('Password is required'),
    confirmPassword: Yup.string()
      .oneOf([Yup.ref('password'), ''], 'Passwords must match')
      .required('Password is required')
    // modeOfContact: Yup.string().required('Required'),
    // phone: Yup.string().when('modeOfContact', {
    //   is: 'telephonemoc',
    //   then: Yup.string().required('Required')
    // })
  })

  const onSubmit = values => {
    console.log('Form data', values)
  }

  return (
    <Formik
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={onSubmit}
    >
      {formik => {
        return (
          <Form>
              <h3>Register</h3>
            <FormikControl
              control='input'
              type='email'
              label='Email'
              name='email'
            />
            <FormikControl
              control='input'
              type='password'
              label='Password'
              name='password'
            />
            <FormikControl
              control='input'
              type='password'
              label='Confirm Password'
              name='confirmPassword'
            />
            {/* <FormikControl
              control='radio'
              label='Mode of contact'
              name='modeOfContact'
              options={options}
            />
            <FormikControl
              control='input'
              type='text'
              label='Phone number'
              name='phone'
            /> */}
            <button type='submit' disabled={!formik.isValid}>
              Submit
            </button>
            <Link type='button' to='/'>Cancel</Link>
          </Form>
        )
      }}
    </Formik>
  )
}

export default RegistrationForm
