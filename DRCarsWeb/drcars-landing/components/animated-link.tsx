"use client"

import { motion } from "framer-motion"
import Link from "next/link"
import type { ReactNode } from "react"

interface AnimatedLinkProps {
  href: string
  children: ReactNode
  className?: string
}

export function AnimatedLink({ href, children, className = "" }: AnimatedLinkProps) {
  return (
    <Link href={href || "#"} className={className}>
      <motion.div
        className="relative inline-block"
        whileHover={{ scale: 1.05 }}
        transition={{ type: "spring", stiffness: 400, damping: 10 }}
      >
        {children}
        <motion.div
          className="absolute bottom-0 left-0 w-full h-0.5 bg-primary origin-left"
          initial={{ scaleX: 0 }}
          whileHover={{ scaleX: 1 }}
          transition={{ duration: 0.3 }}
        />
      </motion.div>
    </Link>
  )
}
