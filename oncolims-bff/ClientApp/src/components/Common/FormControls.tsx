import React from 'react'
import classNames from 'classnames'
import { ExclamationCircleIcon } from '@heroicons/react/solid'
import voca from 'voca';
import { FieldValues, UseFormRegister, DeepMap, FieldError } from 'react-hook-form';
import classnames from 'classnames'

interface FormLabelProps {
  fieldName: string;
  text: string;
  className?: string;
}

interface TextInputProps {
  fieldName: string;
  value: string;
  errors: DeepMap<FieldValues, FieldError>;
  autocomplete?: "off" | "name" | "on" | "given-name" | "honorific-prefix" | undefined;
  register: UseFormRegister<FieldValues>;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  type: "text" | "password" | "email";
  className?: string;
}

function FormLabel({fieldName , text, className}: FormLabelProps){
  let fieldKebab = voca.kebabCase(fieldName);

  return (
    <label htmlFor={fieldKebab} className={classnames(`block text-sm font-medium text-gray-700 sm:mt-px sm:pt-2`, className)}>
      {text}
    </label>
  )
}

function TextInput({fieldName, value, errors, autocomplete = undefined, register, onChange, type = "text", className}: TextInputProps) {
  let fieldKebab = voca.kebabCase(fieldName);

  return (
    <>
      <div className={classnames("relative", className)}>
        <input
          type={type}
          {...register(fieldName)}
          id={fieldKebab}
          autoComplete={autocomplete}
          onChange={onChange}
          value={value}
          className={classNames(
            "max-w-lg block w-full shadow-sm sm:max-w-xs sm:text-sm rounded-md", 
            {"focus:ring-emerald-500 focus:border-emerald-500 border-gray-300": !errors}, 
            {"border-red-300 text-red-900 placeholder-red-300 focus:outline-none focus:ring-red-500 focus:border-red-500": errors})
          }
          aria-invalid={errors ? "true" : "false"}
          aria-describedby={errors ? `invalid-${fieldKebab}` : undefined}
        />
        { errors && (
          <div className="absolute inset-y-0 right-0 pr-3 flex items-center pointer-events-none">
            <ExclamationCircleIcon className="h-5 w-5 text-red-500" aria-hidden="true" />
          </div>
        )}
      </div>
      {errors && <p id={`${fieldKebab}-error`} role="alert" className="mt-1 text-red-600 font-medium text-sm">{errors.message}</p>}
    </>
  )
}

export { FormLabel, TextInput }