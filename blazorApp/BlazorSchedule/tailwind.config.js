/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./**/*.{razor,html}',
    './Pages/**/*.razor',
    './Layout/**/*.razor',
    './wwwroot/**/*.html',
    './Shared/**/*.razor',
    './**/*.razor',
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}

