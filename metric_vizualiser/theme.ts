import { extendTheme } from "@chakra-ui/react";

export const theme = extendTheme({
  styles: {
    global: {
      ".chakra-collapse": {
        overflow: "unset !important",
      },
      body: {
        bg: "rgba(17,18,23,255)",
        color: "white",
        padding: "0",
        margin: "15px",
        fontFamily:
          "-apple-system, BlinkMacSystemFont, Segoe UI, Roboto, Oxygen, Ubuntu, Cantarell, Fira Sans, Droid Sans, Helvetica Neue, sans-serif",
      },
      html: {
        bg: "rgba(17,18,23,255)",
        color: "white",
        padding: "0",
        margin: "15px",
        fontFamily:
          "-apple-system, BlinkMacSystemFont, Segoe UI, Roboto, Oxygen, Ubuntu, Cantarell, Fira Sans, Droid Sans, Helvetica Neue, sans-serif",
      },
    },
  },
  components: {
    Select: {
      baseStyle: {
        bg: "black",
        color: "pink",
      },
    },
    Collapse: {
      baseStyle: {
        overflow: "unset",
      },
    },
    option: {
      color: "black",
    },
  },
});
