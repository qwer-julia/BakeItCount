/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./Pages/**/*.{cshtml,html,razor}",
        "./Views/**/*.{cshtml,html,razor}",
        "./wwwroot/**/*.{js,ts}"
    ], theme: {
        extend: {
            boxShadow: {
                'solid-cocoa': '20px 20px 0 0 #6F5642',
                'solid-peach': '20px 20px 0 0 #C88163',
            },
            colors: {
                beige: {
                    light: "#FAF6EE", // +1 tom mais claro
                    DEFAULT: "#F4EFE1", // original
                    dark: "#DED6C7"   // +1 tom mais escuro
                },
                cocoa: {
                    light: "#87705C",
                    DEFAULT: "#6F5642",
                    dark: "#5A4535"
                },
                peach: {
                    light: "#D99A81",
                    DEFAULT: "#C88163",
                    dark: "#A86A4B"
                },
                sand: {
                    light: "#E8B38C",
                    DEFAULT: "#D9A07D",
                    dark: "#B78163"
                },
                mint: {
                    light: "#7FC9B1",
                    DEFAULT: "#6AA998",
                    dark: "#538373"
                },
            },
            fontFamily: {
                bungeeShade: ["'Bungee Shade'", "cursive"],
                bungeeInline: ["'Bungee Inline'", "cursive"],
                bungee: ["'Bungee'", "cursive"],
            },
        },
    },
    plugins: [],
}

