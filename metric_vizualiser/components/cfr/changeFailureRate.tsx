import { Text } from "@chakra-ui/react";
import * as Plot from "@observablehq/plot";
import { COLORS } from "const";
import { useEffect, useRef } from "react";
import { IChangeFailureRate } from "types";
import { getDateOfWeek } from "utils";

type ChangeFailureRateProps = {
  data?: IChangeFailureRate;
  months: number;
  size: { width: number; height: number };
};

export function ChangeFailureRate({
  data,
  months,
  size,
}: ChangeFailureRateProps) {
  const headerRef = useRef(null);
  useEffect(() => {
    if (data === undefined) {
      return;
    }
    const getmontlyTransformedDataMedian = (data: IChangeFailureRate) =>
      data.changeFailureRateByMonth.map((d) => ({
        date: new Date(d.key.yearNumber, d.key.monthNumber),
        median: d.changeFailureRate * 100,
      }));
    const getweeklyTransformedDataMedian = (data: IChangeFailureRate) =>
      data.changeFailureRateByWeek.map((d) => ({
        date: getDateOfWeek(d.key.weekNumber, d.key.yearNumber),
        median: d.changeFailureRate * 100,
      }));

    const getDailyTransformedDataMedian = (data: IChangeFailureRate) =>
      data.changeFailureRateByDay.map((d) => ({
        date: new Date(d.key.yearNumber, d.key.monthNumber, d.key.dayNumber),
        median: d.changeFailureRate * 100,
      }));

    const transformedDataMedian =
      months < 2
        ? getmontlyTransformedDataMedian(data)
        : months < 4
        ? getweeklyTransformedDataMedian(data)
        : getDailyTransformedDataMedian(data);

    const chart = Plot.plot({
      style: {
        background: COLORS.PAPER,
      },
      y: {
        grid: true,
        label: "Percentage %",
      },
      color: {
        type: "diverging",
        scheme: "burd",
        range: ["red", "blue"],
        interpolate: "hcl",
      },
      marks: [
        Plot.ruleY([0]),
        Plot.line(transformedDataMedian, {
          x: "date",
          y: "median",
          curve: "basis",
          marker: "circle",
          stroke: COLORS.BLUE,
          opacity: 1,
        }),
      ],
      width: size.width,
      height: size.height,
    });
    // @ts-ignore
    headerRef.current.append(chart);
    return () => chart.remove();
  }, [data, months, size]);
  return (
    <div
      style={{
        justifyContent: "space-between",
        display: "flex",
        flexDirection: "column",
        height: "100%",
        margin: "0 10px",
      }}
    >
      <Text fontSize="2xl">{"Change Failure Rate"}</Text>
      <div ref={headerRef} style={{ marginBottom: "20px" }} />
    </div>
  );
}
