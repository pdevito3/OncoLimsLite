const colors = require('tailwindcss/colors')

module.exports = {
  mode: 'jit',
  purge: [
    './public/**/*.html',
    './src/**/*.{js,jsx,ts,tsx}',
  ],
  darkMode: false, // or 'media' or 'class'
  theme: {
    extend: {
      colors:{
        violet: colors.violet,
        emerald: colors.emerald,
      },
      padding:{
        "4.5": "1.125rem"
      },
      maxWidth: {
        '8xl': '88rem',
        '9xl': '96rem',
      },
    },
  },
  variants: {
    extend: {
      display: ['group-focus', 'group-hover'],
      backgroundColor: ['even'],
    }
  },
  plugins: [
    require('@tailwindcss/forms'),
  ],
}
