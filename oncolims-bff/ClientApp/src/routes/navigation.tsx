import {
  UsersIcon,
  CubeTransparentIcon,
} from '@heroicons/react/outline'

export const navigation = [
  { name: 'Patients', exact: true, href: '/', icon: UsersIcon },
  { name: 'Samples', exact: false, href: '/samples', icon: CubeTransparentIcon },
];