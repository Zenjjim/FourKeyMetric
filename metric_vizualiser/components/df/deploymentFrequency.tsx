import { COLORS } from "const";
import * as d3 from "d3";
import { useState } from "react";
import { Bar, BarChart, CartesianGrid, Cell, Label, ResponsiveContainer, Tooltip, XAxis, YAxis } from "recharts";
import { Deployment, DeploymentsInBucket, IDeploymentFrequency } from "types";
import { getDateOfWeek } from "utils";
type DeploymentFrequencyProps = {
  data: IDeploymentFrequency;
  months: number;
  size: { width: number; height: number };
};
interface Map {
  [key: string]: DeploymentsInBucket[]
}
export function DeploymentFrequency({
  data,
  months,
  size,
}: DeploymentFrequencyProps) {
  console.log(data)

  const daily = 2
  const weekly = 6
  const getDaily = (data: IDeploymentFrequency) => {
    return data.deployments.map(deployment => {
      deployment["date"] = new Date(deployment.yearNumber, deployment.monthNumber-1, deployment.dayNumber)
      deployment["count"] = deployment.deploymentsInBucket.length
      return deployment
    })
  }

  const getWeekly = (data: IDeploymentFrequency) => {
    const temp: Deployment[] = []
    data.deployments.forEach(deployment => {
      const found = temp.find(t => t.weekNumber === deployment.weekNumber && t.yearNumber === deployment.yearNumber)
      if (found){
        found.deploymentsInBucket = found.deploymentsInBucket.concat(deployment.deploymentsInBucket)
      } else {
        temp.push({
          date: getDateOfWeek(deployment.weekNumber as number, deployment.yearNumber),
          weekNumber: deployment.weekNumber,
          monthNumber: deployment.monthNumber,
          yearNumber: deployment.yearNumber,
          deploymentsInBucket: deployment.deploymentsInBucket
        })
      }
    })
    return temp.map(deployment => {
      deployment["count"] = deployment.deploymentsInBucket.length
      return deployment
    })
  }
  const getMonthly = (data: IDeploymentFrequency) => {
    const temp: Deployment[] = []
    data.deployments.forEach(deployment => {
      const found = temp.find(t => t.monthNumber === deployment.monthNumber && t.yearNumber === deployment.yearNumber)
      if (found) {
        found.deploymentsInBucket = found.deploymentsInBucket.concat(deployment.deploymentsInBucket)
      } else {
        temp.push({
          date: new Date(deployment.yearNumber, deployment.monthNumber-1, 1),
          monthNumber: deployment.monthNumber,
          yearNumber: deployment.yearNumber,
          deploymentsInBucket: deployment.deploymentsInBucket
        })
      }
    })
    return temp.map(deployment => {
      deployment["count"] = deployment.deploymentsInBucket.length
      return deployment
    })
  }
  const displayData: Deployment[] = (months < daily) ? getDaily(data) : (months < weekly) ? getWeekly(data) : getMonthly(data)
  
  const CustomTooltip = ({ active, payload, label }:{ active:any, payload:any, label:any } ) => {
    if (active && payload && payload.length) {
      return (
        <div style={{backgroundColor: COLORS.PAPER, padding: "5px", borderRadius: "10px"}}>
          <p className="label">{`Deployments : ${payload[0].value}`}</p>
          <p className="date">{`Date : ${label.toDateString()}`}</p>
        </div>
      );
    }
    return null;
  };
  

  return (
    <ResponsiveContainer width="100%" height="100%">
      <BarChart data={displayData}>
        <CartesianGrid vertical={false} />
        <XAxis dataKey="date" tickFormatter={d3.timeFormat("%d %B")} />
        <YAxis />
        <Tooltip cursor={{fill: 'rgba(255, 255, 255, 0.1)'}} content={<CustomTooltip active={undefined} payload={undefined} label={undefined} />} />
        <Bar dataKey="count" fill={COLORS.BLUE} />
      </BarChart>
    </ResponsiveContainer>
  )
}
