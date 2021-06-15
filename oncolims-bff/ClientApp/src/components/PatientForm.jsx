import React from 'react'
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import { DevTool } from "@hookform/devtools";
import { FormLabel, TextInput } from '../components/Common/FormControls'

const defaultFormValues = {
  firstName: '',
  lastName: ''
}

const patientSchema = yup.object().shape({
  firstName: yup.string().required().label('First name'),
  lastName: yup.string().label('Last name')
});

function PatientForm({
  onSubmit,
  initialValues = defaultFormValues,
  submitText,
  clearOnSubmit,
  setIsOpen,
  resetMutation
}) {  
  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
    reset
  } = useForm({
    resolver: yupResolver(patientSchema),
    mode: "onChange"
  });
  const [values, setValues] = React.useState(initialValues)
  const setValue = (field, value) =>
    setValues((old) => ({ ...old, [field]: value }))

  const internalHandleSubmit = async (e) => {
    if (clearOnSubmit) {
      setValues(defaultFormValues)
    }

    await onSubmit(values)
    reset()
    window.setTimeout(() => resetMutation(), 1500)
  }

  React.useEffect(() => {
    setValues(initialValues)
  }, [initialValues])
  
  return (
    <div>    
      {/* <DevTool control={control} placement={"top-right"} />    */}
      <div>
          <div>
            <h3 className="text-lg leading-6 font-medium text-gray-900">
              Patient Information
            </h3>
          </div>

          <form className="mt-6 sm:mt-5" 
            onSubmit={handleSubmit(internalHandleSubmit)} 
            onKeyDown={(e) => {if(e.key === "Enter") handleSubmit(internalHandleSubmit(e))}} 
          >
            <div className="sm:grid sm:grid-cols-3 sm:gap-4 sm:items-start sm:border-t sm:border-gray-200 sm:pt-5">
              <FormLabel text="First Name" fieldName="firstName" />
              <div className="mt-1 sm:mt-0 sm:col-span-2">
                <TextInput fieldName="firstName" value={values.firstName} formFieldErrors={errors.firstName} autocomplete="given-name" register={register} setValue={setValue} />
              </div>

              <FormLabel text="Last Name" fieldName="lastName" />
              <div className="mt-1 sm:mt-0 sm:col-span-2">
                <TextInput fieldName="lastName" value={values.lastName} formFieldErrors={errors.lastName} autocomplete="given-name" register={register} setValue={setValue} />
              </div>
            </div>
          </form>
        </div>
        
        <div className="mt-5 sm:mt-6 space-y-2">
          <span className="flex w-full rounded-md shadow-sm">
            <button onClick={handleSubmit(internalHandleSubmit)} type="submit" className="inline-flex justify-center w-full rounded-md border border-transparent px-4 py-2 bg-emerald-600 text-base leading-6 font-medium text-white shadow-sm hover:bg-emerald-500 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-emerald-500 transition ease-in-out duration-150 sm:text-sm sm:leading-5">
              {submitText}              
            </button>
          </span>

          <span className="flex w-full rounded-md shadow-sm">
            <button onClick={() => setIsOpen(false)} type="button" className="inline-flex justify-center w-full rounded-md border border-transparent px-4 py-2 bg-white text-base leading-6 font-medium text-gray-500 shadow-sm hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500 transition ease-in-out duration-150 sm:text-sm sm:leading-5">
              Cancel
            </button>
          </span>
        </div>
    </div>
  )
}

export default PatientForm
