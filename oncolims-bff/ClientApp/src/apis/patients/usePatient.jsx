import axios from 'axios';
import { useQuery } from 'react-query';
import { patientsBaseUrl, queryClient, patientKeys } from '../constants'

const config = {
  headers: {}
}

const fetchPatient = async (patientId) =>
axios.get(`${patientsBaseUrl}/${patientId}`, config)
    .then((res) => res.data);

export default function usePatient(patientId) {
return useQuery(
    patientKeys.detail(patientId),
    async () => fetchPatient(patientId),
    {
      enabled: false // prevents it from running by default when it will be null
    }
)}