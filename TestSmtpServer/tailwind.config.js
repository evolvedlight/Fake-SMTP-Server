module.exports = {
    content: ["./Pages/**/*.{html,js,cshtml,razor}", "./Shared/**/*.{html,js,cshtml,razor}"],
    theme: {
        extend: {},
    },
    plugins: [
        require('@tailwindcss/forms'),
    ],
}