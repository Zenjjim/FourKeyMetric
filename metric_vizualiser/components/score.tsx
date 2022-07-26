import { Text } from "@chakra-ui/react";
import { COLORS } from "const";

type ScoreProps = {
  category: string;
  color: string;
  title: string;
  textSize: number;
};
export function Score({ category, color, title, textSize }: ScoreProps) {
  return (
    <div style={{ width: "100%", position: "relative" }}>
      <Text
        fontSize="2xl"
        style={{ color: COLORS.WHITE, position: "absolute", width: "100%" }}
      >
        {title}
      </Text>
      <div
        style={{
          alignItems: "center",
          display: "flex",
          justifyContent: "center",
          width: "100%",
          height: "100%",
        }}
      >
        <div style={{ color: color, fontSize: `${textSize}px`, width: "100%" }}>
          {category}
        </div>
      </div>
    </div>
  );
}