
import { QueryClient } from 'react-query';

const baseUrl = "https://localhost:1200/api";

export const queryClient = new QueryClient();

export const patientsBaseUrl = `${baseUrl}/patients`;

export const patientKeys = {
  all: ['patients'],
  lists: () => [...patientKeys.all, 'list'],
  list: (filters: string | null | undefined, pageNumber: number) => [...patientKeys.lists(), filters ?? '' , pageNumber],
  details: () => [...patientKeys.all, 'detail'],
  detail: (id: number) => [...patientKeys.details(), id],
}