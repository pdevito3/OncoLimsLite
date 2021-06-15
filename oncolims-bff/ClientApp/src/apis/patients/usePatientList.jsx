import axios from 'axios';
import { useQuery } from 'react-query';
import { patientsBaseUrl, queryClient, patientKeys } from '../constants'
import {
    encodeQueryParams,
    StringParam,
    NumberParam,
  } from 'serialize-query-params';
  import { stringify } from 'query-string';

const config = {
    headers: {}
}

export let pagination;
const fetchPatients = async ({pageNumber = 1, filter}) => {
    let qp = encodeQueryParams(
        {pageSize: NumberParam, pageNumber: NumberParam, filters: StringParam}, 
        {pageSize: 6, pageNumber: pageNumber, ...(filter !== undefined) && {filters: `firstName@=*${filter}`}}
    )
    let queryParams = stringify(qp);

    let res = await axios.get(`${patientsBaseUrl}?${queryParams}`, config);
    
    // TODO move pagination to DTO body in api to cache the data
    pagination = JSON.parse(res.headers["x-pagination"]);

    return res.data;
}

// TODO Update to infinite query to fetch next page automatically
export function usePatients({pageNumber, filter}) {
    return useQuery(
        patientKeys.list(filter, pageNumber),
        async () => fetchPatients({pageNumber, filter}),
        {
            keepPreviousData: true,
            staleTime: Infinity,
            cacheTime: Infinity,
            onSuccess: patients => {
                // add individual patients to cache
                for (const patient of patients.data) {
                  queryClient.setQueryData(
                    patientKeys.detail(patient.patientId),
                    patient
                  )
                }
            }
        }
    )
}