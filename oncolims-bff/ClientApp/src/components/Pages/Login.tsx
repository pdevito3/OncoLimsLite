import React from 'react'
import { TextInput } from '../Common/FormControls/TextInput'
import { FormLabel } from '../Common/FormControls/FormLabel'
import { useForm, SubmitHandler, FieldValues } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import { DevTool } from '@hookform/devtools';

const defaultFormValues = {
  username: '',
  password: '',
  remember: false
}

const loginSchema = yup.object().shape({
  username: yup.string().required().label('Username'),
  password: yup.string().label('Password')
});

function Login() {
  const [data, setData] = React.useState(defaultFormValues)
  const setValue = (field: string, value: string) =>
    setData((old) => ({ ...old, [field]: value }))

  const { register, handleSubmit, control, formState: { errors }, reset} = useForm({
    resolver: yupResolver(loginSchema),
    mode: "onChange"
  });
  const onSubmit = (data: SubmitHandler<FieldValues>) => console.log(data);

  return (
    <div className="flex items-center justify-center min-h-screen p-6 bg-emerald-900">
      {/* <Helmet title="Login" /> */}
      <div className="w-full max-w-md">
        {/* <Logo
          className="block w-full max-w-xs mx-auto text-white fill-current"
          height={50}
        /> */}
        <form
          onSubmit={handleSubmit(() => console.log("submit"))}
          className="mt-8 overflow-hidden bg-white rounded-lg shadow-xl"
        >
          <div className="px-10 py-12">
            <h1 className="text-3xl font-bold text-center">Welcome Back!</h1>
            <div className="w-24 mx-auto mt-6 border-b-2" />
            
            <FormLabel text="Username" fieldName="username" />
            <div className="mt-1 sm:mt-0">
              <TextInput
                fieldName="username"
                type="text"
                errors={errors.username}
                defaultValue={data.username}
                register={register}
                onChange={(e) => setValue("username", e.target.value)}
                />
            </div>

            
            <FormLabel text="Password" fieldName="password" />
            <div className="mt-1 sm:mt-0">
              <TextInput
                fieldName="password"
                type="password"
                errors={errors.password}
                defaultValue={data.password}
                register={register}
                onChange={(e) => setValue("password", e.target.value)}
              />
            </div>
            <label
              className="flex items-center mt-4 select-none"
              htmlFor="remember"
            >
              <input
                name="remember"
                id="remember"
                className="h-4 w-4 text-emerald-600 focus:ring-emerald-500 border-gray-300 rounded mr-1"
                type="checkbox"
              />
              <span className="ml-2 block text-sm text-gray-900">Remember Me</span>
            </label>
          </div>
          <div className="flex items-center justify-between px-10 py-4 bg-gray-100 border-t border-gray-200">
            <a className="hover:underline" tabIndex={-1} href="#reset-password">
              Forgot password?
            </a>
            {/* <LoadingButton
              type="submit"
              loading={processing}
              className="btn-indigo"
            >
              Login
            </LoadingButton> */}
            {/* <button type="submit">Login TBD</button> */}
              <a 
              href="/bff/login?returnUrl=/"
                className="w-full inline-block bg-emerald-500 py-2 px-4 border border-transparent rounded-md text-base text-center font-medium text-white hover:bg-opacity-75"
              >
                Login
              </a>
          </div>
        </form>
      </div>
      <DevTool control={control} placement={"top-left"} />
    </div>
  );
}

export default Login
