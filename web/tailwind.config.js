module.exports = {
    content: [
        "./pages/**/*.{ts,tsx}",
        "./components/**/*.{ts,tsx}"
    ],
    theme: {
        extend: {
            keyframes: {
                'fade-in-down': {
                    '0%': { transform: 'translateY(-10px)' },
                    '100%': { transform: 'translateY(0)' },
                },
                'fade-out-up': {
                    '0%': {
                        opacity: '1',
                        transform: 'translateY(0px)'
                    },
                    '100%': {
                        opacity: '0',
                        transform: 'translateY(-10px)'
                    },
                },
            },
            animation: {
                'fade-in-down': 'fade-in-down .3s linear',
                'fade-out-up': 'fade-out-up .3s linear',
            }
        },
    },
    plugins: [],
}
