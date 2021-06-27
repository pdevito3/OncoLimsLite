import React from 'react'
import classnames from 'classnames'
import voca from 'voca';

interface FormLabelProps {
  fieldName: string;
  text: string;
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

export { FormLabel }
