/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./NoBullshitTimer/**/*.{razor,html}'],
  theme: {
    extend: {},
    screens: {
      sm: "640",
      md: "768",
      lg: "1024",
      xl: "1280",
      "2xl": "1536"
    }
  },
  plugins: [],
}

