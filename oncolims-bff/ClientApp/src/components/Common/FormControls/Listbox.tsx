import React, { useState, Fragment } from 'react'
import classNames from 'classnames'
import { FieldValues, UseFormRegister, DeepMap, FieldError, Controller, Control, ControllerRenderProps } from 'react-hook-form';
import { Listbox as HeadlessListbox, Transition } from '@headlessui/react'
import { CheckIcon, SelectorIcon } from '@heroicons/react/solid'

interface ListboxProps {
  fieldName: string;
  value: string;
  errors: DeepMap<FieldValues, FieldError>;
  control: Control<FieldValues>;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  data: { id: string | null | undefined, text: string | null | undefined }[];
}

function Listbox({fieldName, value, errors, control, onChange, data}: ListboxProps) {  
  return (
    <Controller 
      name={fieldName}
      control={control}
      render={({field}) => {
        field.value = value;

        return (<MySelect
          data={data}
          field={{...field}}
          onChange={onChange}
        /> )
      }
    }/>
  )
}

interface MySelectProps {
  field: ControllerRenderProps<FieldValues, string>;
  data: { id: string | null | undefined, text: string | null | undefined }[];
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

function MySelect({ data, field, onChange }: MySelectProps){
  const [selected, setSelected] = useState(data?.find(i => i?.id === field?.value))
  
  React.useEffect(() => {
    setSelected(field?.value)
  }, [field.value])

  return (
    <HeadlessListbox value={field.value} onChange={(e) => {field.onChange(e); onChange(e);}} >
      {({ open }) => (
        <>
          <div className="mt-1 relative">
            <HeadlessListbox.Button 
              className={
                classNames("relative w-full bg-white border border-gray-300 rounded-md shadow-sm pl-3 pr-10 text-left cursor-default focus:outline-none focus:ring-1 focus:ring-emerald-500 focus:border-emerald-500 sm:text-sm",
                  String(field.value).length <= 0 || field.value == null ? "py-4.5" : "py-2" // need `== null` to capture undefined -- make a test! 
                )
              }
            >
              <span className="block truncate">{selected?.text ?? selected}</span>
              <span className="absolute inset-y-0 right-0 flex items-center pr-2 pointer-events-none">
                <SelectorIcon className="h-5 w-5 text-gray-400" aria-hidden="true" />
              </span>
            </HeadlessListbox.Button>

            <Transition
              show={open}
              as={Fragment}
              leave="transition ease-in duration-100"
              leaveFrom="opacity-100"
              leaveTo="opacity-0"
            >
              <HeadlessListbox.Options
                static
                className="absolute z-10 mt-1 w-full bg-white shadow-lg max-h-60 rounded-md py-1 text-base ring-1 ring-black ring-opacity-5 overflow-auto focus:outline-none sm:text-sm"
              >
                {data.map((record) => (
                  <HeadlessListbox.Option
                    key={record.id}
                    className={({ active }) =>
                      classNames(
                        active ? 'text-white bg-emerald-600' : 'text-gray-900',
                        'cursor-default select-none relative py-2 pl-8 pr-4'
                      )
                    }
                    value={record?.text}
                  >
                    {({ selected, active }) => (
                      <>
                        <span className={classNames(selected ? 'font-semibold' : 'font-normal', 'block truncate',
                          record?.text === null ? "py-2.5" : ""
                        )}>
                          {record?.text}
                        </span>

                        {selected ? (
                          <span
                            className={classNames(
                              active ? 'text-white' : 'text-emerald-600',
                              'absolute inset-y-0 left-0 flex items-center pl-1.5'
                            )}
                          >
                            <CheckIcon className="h-5 w-5" aria-hidden="true" />
                          </span>
                        ) : null}
                      </>
                    )}
                  </HeadlessListbox.Option>
                ))}
              </HeadlessListbox.Options>
            </Transition>
          </div>
        </>
      )}
    </HeadlessListbox>
  )
}

export { Listbox }