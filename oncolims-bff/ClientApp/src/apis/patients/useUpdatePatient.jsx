import axios from 'axios';
import { useMutation } from 'react-query';
import { patientsBaseUrl, queryClient, patientKeys } from '../constants'

const config = {
  headers: {}
}

export default function usePutPatient() {
return useMutation(
  (values) => 
    axios.put(`${patientsBaseUrl}/${values.patientId}`, values), 
    {
      onMutate: (values) => {
        queryClient.cancelQueries(patientKeys.lists)

        // const oldPost = queryCache.getQueryData(['posts', values.id])

        // queryCache.setQueryData(['posts', values.id], values)

        // return () => queryCache.setQueryData(['posts', values.id], oldPost)
      },
      onError: (error, values, rollback) => rollback(),
      onSuccess: (data, variables) => {
        queryClient.invalidateQueries(patientKeys.lists)
        queryClient.invalidateQueries(patientKeys.detail(values.patientId))
      },
    }
)}