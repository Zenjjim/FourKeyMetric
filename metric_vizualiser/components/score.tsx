import { COLORS } from "const";
import * as d3 from "d3";
import { useEffect, useRef } from "react";
import { ILeadTimeChange } from "types";
import { getHoursfromSeconds } from "utils";
type ScoreProps = {
  category: string;
  color: string;
  title: string;
  textSize: number;
};
export function Score({ category, color, title, textSize }: ScoreProps) {
  return (
    <div style={{ width: "100%", position: "relative" }}>
      <h3 style={{ color: COLORS.WHITE, position: "absolute", width: "100%" }}>{title}</h3>
      <div style={{ alignItems: "center", display: "flex", justifyContent: "center", width: "100%", height: "100%" }}>
        <div style={{ color: color, fontSize: `${textSize}px`, width: "100%" }}>
          {category}
        </div>
      </div>
    </div>
  );
}
