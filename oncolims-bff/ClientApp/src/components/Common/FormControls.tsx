import React from 'react'
import classNames from 'classnames'
import { ExclamationCircleIcon } from '@heroicons/react/solid'
import voca from 'voca';
import { FieldValues, UseFormRegister, DeepMap, FieldError } from 'react-hook-form';

interface FormLabelProps {
  fieldName: string;
  text: string;
  className: string;
}

interface TextInputProps {
  fieldName: string;
  value: string;
  formFieldErrors: DeepMap<FieldValues, FieldError>;
  autocomplete: "off" | "name" | "on" | "given-name" | "honorific-prefix";
  register: UseFormRegister<FieldValues>;
  setValue: (field:string, value:string) => void;
}

function FormLabel({fieldName , text, className}: FormLabelProps){
  let fieldKebab = voca.kebabCase(fieldName);

  return (
    <label htmlFor={fieldKebab} className={`block text-sm font-medium text-gray-700 sm:mt-px sm:pt-2 ${className}`}>
      {text}
    </label>
  )
}

function TextInput({fieldName, value, formFieldErrors, autocomplete, register, setValue}: TextInputProps) {
  let fieldKebab = voca.kebabCase(fieldName);

  return (
    <>
      <div className="relative">
        <input
          type="text"
          {...register(fieldName)}
          id={fieldKebab}
          autoComplete={autocomplete}
          onChange={(e) => setValue(fieldName, e.target.value)}
          value={value}
          className={classNames(
            "max-w-lg block w-full shadow-sm sm:max-w-xs sm:text-sm rounded-md", 
            {"focus:ring-emerald-500 focus:border-emerald-500 border-gray-300": !formFieldErrors}, 
            {"border-red-300 text-red-900 placeholder-red-300 focus:outline-none focus:ring-red-500 focus:border-red-500": formFieldErrors})
          }
          aria-invalid={formFieldErrors ? "true" : "false"}
          aria-describedby={formFieldErrors ? `invalid-${fieldKebab}` : undefined}
        />
        { formFieldErrors && (
          <div className="absolute inset-y-0 right-0 pr-3 flex items-center pointer-events-none">
            <ExclamationCircleIcon className="h-5 w-5 text-red-500" aria-hidden="true" />
          </div>
        )}
      </div>
      {formFieldErrors && <p id={`${fieldKebab}-error`} role="alert" className="mt-1 text-red-600 font-medium text-sm">{formFieldErrors.message}</p>}
    </>
  )
}

export { FormLabel, TextInput }