
export module Theme {
    export const DEFAULT_THEME = "light";
    
    export function getTheme(): string {
        return document.body.classList.contains("bootstrap-dark") ? "dark" : "light";
    }

    export function setTheme(theme: string) {
        let currentTheme = getTheme();
        switch(theme) {
            case "light":
                if(currentTheme === "dark") {
                    document.body.classList.remove("bootstrap-dark");
                    document.body.classList.add("bootstrap");
                }
                break;
            case "dark":
                if(currentTheme === "light") {
                    document.body.classList.remove("bootstrap");
                    document.body.classList.add("bootstrap-dark");
                }
                break;
        }
    }

    export function switchTheme() {
        setTheme(getTheme() === "dark" ? "light" : "dark");
    }
}

export function onLoad(ev: Event) {
    let theme = localStorage.getItem("theme");
    Theme.setTheme(theme !== null ? theme : Theme.DEFAULT_THEME);
}