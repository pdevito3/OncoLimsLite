import React from 'react'
import axios from 'axios'
import { patientsBaseUrl, queryClient, patientKeys } from '../constants'
import { useMutation } from 'react-query'

export default function useDeletePost() {
  return useMutation(
    (patientId) => axios.delete(`${patientsBaseUrl}/${patientId}`),
    {
      onSuccess: () => queryClient.invalidateQueries(patientKeys.list),
    }
  )
}