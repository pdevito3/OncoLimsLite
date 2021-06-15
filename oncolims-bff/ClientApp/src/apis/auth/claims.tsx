import axios from 'axios';
import { useQuery } from 'react-query';

const claimsKeys = {
  claim: ['claim']
  // claims: ['claims'],
  // claims: (id) => [...claimsKeys.claims, id]
}

const config = {
  headers: {
    'X-CSRF': '1'
  }
}

const fetchClaims = async (): Promise<Record<string, string>[]> => 
  axios.get('/bff/user', config)
    .then((res) => res.data);


export default function useUser() {
  return useQuery(
    claimsKeys.claim, 
    async () => fetchClaims(),
    {
    }
  )
}